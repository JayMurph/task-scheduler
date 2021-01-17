﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

using task_scheduler_entities;

using task_scheduler_application;
using task_scheduler_application.DTO;
using task_scheduler_application.NotificationFrequencies;
using task_scheduler_application.UseCases.CreateTask;
using task_scheduler_application.UseCases.ViewTasks;
using task_scheduler_application.UseCases.ViewNotifications;

using task_scheduler_data_access;
using task_scheduler_data_access.Repositories;
using task_scheduler_data_access.DataObjects;

using task_scheduler_presentation.Models;

namespace task_scheduler_presentation
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Called upon to execute application functionality in the presentation layer
        /// </summary>
        static public Controllers.UserController UserController;

        /// <summary>
        /// name of the database file to store and retrieve application data from
        /// </summary>
        public const string DATA_SOURCE_FILENAME = "TaskSchedulerDB.db";

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e) {

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null) {

                await CreateDatabaseFileIfNecessary(DATA_SOURCE_FILENAME);

                /*
                 * initialize a database connection string that will point to a database file in
                 * the applications app data storage
                 */
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DATA_SOURCE_FILENAME);
                string dbConnectionStr = $"Data Source={dbPath};";

                DataAccess.InitializeDatabase(dbConnectionStr);

                //CREATE REPOSITORY FACTORIES
                NotificationFrequencyRepositoryFactory notificationFrequencyRepositoryFactory =
                    new NotificationFrequencyRepositoryFactory(dbConnectionStr);

                TaskItemRepositoryFactory taskItemRepositoryFactory =
                    new TaskItemRepositoryFactory(dbConnectionStr, notificationFrequencyRepositoryFactory);

                NotificationRepositoryFactory notificationRepositoryFactory =
                    new NotificationRepositoryFactory(dbConnectionStr);

                //create domain dependencies
                BasicNotificationManager notificationManager = new BasicNotificationManager();
                BasicTaskManager taskManager = new BasicTaskManager();
                RealTimeClock clock = new RealTimeClock();

                InitializeDomainFromDatabase(
                    taskItemRepositoryFactory,
                    notificationRepositoryFactory,
                    notificationManager,
                    taskManager,
                    clock
                );

                InitializeUserController(
                    taskItemRepositoryFactory,
                    notificationManager,
                    taskManager,
                    clock
                );

                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(Views.MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Creates the static UserController used by the class
        /// </summary>
        /// <param name="taskItemRepositoryFactory"></param>
        /// <param name="notificationManager"></param>
        /// <param name="taskManager"></param>
        /// <param name="clock"></param>
        private void InitializeUserController(
            TaskItemRepositoryFactory taskItemRepositoryFactory,
            BasicNotificationManager notificationManager,
            BasicTaskManager taskManager,
            RealTimeClock clock) {

            //CREATE USE-CASE FACTORIES
            var createTaskUseCaseFactory =
                new CreateTaskUseCaseFactory(
                    taskManager,
                    notificationManager,
                    clock,
                    taskItemRepositoryFactory
                );

            var viewTasksUseCaseFactory =
                new ViewTasksUseCaseFactory(taskManager, taskItemRepositoryFactory);

            var viewNotificationsUseCaseFactory =
                new ViewNotificationsUseCaseFactory(
                    notificationManager,
                    taskItemRepositoryFactory
                );

            //Instantiate user controller, passing in required factories
            UserController = new Controllers.UserController(
                createTaskUseCaseFactory,
                viewTasksUseCaseFactory,
                viewNotificationsUseCaseFactory
            );

            //TODO: find a better solution than this filthy hack
            /*
             * hook up UserController to the notificationManager, so that it is made aware of when
             * the domain creates new notifications
             */
            notificationManager.NotificationAdded +=
                async (s, notification) => {

                    //convert ITaskItem Producer to TaskItem which carries a color
                    if (notification.Producer is TaskItem task) {
                        NotificationDTO dto = new NotificationDTO() {
                            TaskId = task.ID,
                            Time = notification.Time,
                            Title = notification.Producer.Title,
                            R = task.Colour.R,
                            G = task.Colour.G,
                            B = task.Colour.B
                        };

                        await UserController.ReceiveNotification(dto);
                    }
                };
        }

        /// <summary>
        /// Creates and adds neccessary domain objects to the domain manager classes
        /// </summary>
        /// <param name="taskItemRepositoryFactory"></param>
        /// <param name="notificationRepositoryFactory"></param>
        /// <param name="notificationManager"></param>
        /// <param name="taskManager"></param>
        /// <param name="clock"></param>
        private void InitializeDomainFromDatabase(
            TaskItemRepositoryFactory taskItemRepositoryFactory,
            NotificationRepositoryFactory notificationRepositoryFactory,
            BasicNotificationManager notificationManager,
            BasicTaskManager taskManager,
            RealTimeClock clock) {

            //...pulling in data and creating domain entities should be done elsewhere
            //load database data into domain managers
            ITaskItemRepository taskItemRepository = taskItemRepositoryFactory.New();

            //read in task items from database. Create domain taskItems from 
            //data and add items to taskManager
            foreach (TaskItemDAL task in taskItemRepository.GetAll()) {

                INotificationFrequency notificationFrequency =
                    NotificationFrequencyFactory.New(
                        //TODO: do something safer than just a cast
                        (NotificationFrequencyType)task.NotificationFrequencyType,
                        //TODO: do something more sensible than below
                        (task.CustomNotificationFrequency?.Time ?? TimeSpan.Zero)
                    );

                taskManager.Add(
                    new TaskItem(
                        task.Title,
                        task.Description,
                        new Colour(task.R, task.G, task.B),
                        task.StartTime,
                        notificationManager,
                        notificationFrequency,
                        clock,
                        task.LastNotificationTime,
                        task.Id
                    )
                );
            }

            INotificationRepository notificationRepo = notificationRepositoryFactory.New();

            /*
             * read in notifications from database, create domain Notifications from data and store
             * them in the NotificationManager
             */
            foreach(NotificationDAL notification in notificationRepo.GetAll()) {
                ITaskItem producer = taskManager.Find(notification.TaskId);

                if(producer == null) {
                    continue;
                }

                notificationManager.Add(
                    new Notification(producer, notification.Time)
                );
            }
        }

        #region BoilerPlate

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        #endregion


        private static async Task CreateDatabaseFileIfNecessary(string filename) {
            //check if database file already exists
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync(filename) == null) {
                //create and initialize database file if it does not exist
                await ApplicationData.Current.LocalFolder.CreateFileAsync(filename);
            }
        }


    }
}
