using System;
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
using task_scheduler_application.Frequencies;
using task_scheduler_application.UseCases.CreateTask;
using task_scheduler_application.UseCases.ViewTasks;

using task_scheduler_data_access_standard;
using task_scheduler_data_access_standard.Repositories;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_presentation
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        static public Controllers.UserController UserController;
        static public string dataSource = "TaskSchedulerDB.db";
        static public string connectionStr = string.Empty;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            //create database filename path 
            string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, dataSource);
            connectionStr = $"Data Source={dbPath};";

            CreateDatabaseFileIfNecessary().Wait();//TODO go somewhere else

            //Instantiate user controller, passing in required factories
            UserController = CreateUserController();
        }

        private static async Task CreateDatabaseFileIfNecessary() {
            //check if database file already exists
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync(dataSource) == null) {
                //create and initialize database file if it does not exist
                await ApplicationData.Current.LocalFolder.CreateFileAsync(dataSource);
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;


            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
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
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

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


        static private Controllers.UserController CreateUserController() {
            DataAccess.InitializeDatabase(connectionStr);

            //CREATE REPOSITORY FACTORIES
            TaskItemRepositoryFactory taskItemRepositoryFactory = 
                new TaskItemRepositoryFactory(connectionStr);
            FrequencyRepositoryFactory frequencyRepositoryFactory =
                new FrequencyRepositoryFactory(connectionStr);


            //create domain dependencies
            BasicNotificationManager notificationManager = new BasicNotificationManager();
            BasicTaskManager taskManager = new BasicTaskManager();
            RealTimeClock clock = new RealTimeClock();

            //...pulling in data and creating domain entities should be done elsewhere
            //load database data into domain managers

            ITaskItemRepository taskItemRepository = taskItemRepositoryFactory.New();
            IFrequencyRepository frequencyRepository = frequencyRepositoryFactory.New();

            //read in task items from database. Create domain taskItems from 
            //data and add items to taskManager
            foreach(TaskItemDAL task in taskItemRepository.GetAll()) {

                INotificationFrequency notificationFrequency = null;

                //TODO abstract out magic string
                if(task.FrequencyType == "Custom") {

                    NotificationFrequencyDAL notificationFrequencyDAL = 
                        frequencyRepository.GetById(task.Id);

                    notificationFrequency = 
                        NotificationFrequencyFactory.New(task.FrequencyType, notificationFrequencyDAL.Time);
                }
                else {
                    notificationFrequency = NotificationFrequencyFactory.New(task.FrequencyType);
                }

                taskManager.Add(
                    new TaskItem(
                        task.Title,
                        task.Description,
                        new Colour(task.R, task.G, task.B),
                        task.StartTime,
                        notificationManager,
                        notificationFrequency,
                        clock
                    )
                );
            }

            //CREATE USE-CASE FACTORIES
            var createTaskUseCaseFactory =
                new CreateTaskUseCaseFactory(
                    taskManager,
                    notificationManager,
                    clock,
                    taskItemRepositoryFactory,
                    frequencyRepositoryFactory
                );

            var viewTasksUseCaseFactory =
                new ViewTasksUseCaseFactory(taskManager);

            //Instantiate user controller, passing in required factories
            return new Controllers.UserController(
                createTaskUseCaseFactory,
                viewTasksUseCaseFactory
            );
        }

    }
}
