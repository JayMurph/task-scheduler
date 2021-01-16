using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using task_scheduler_application.UseCases.CreateTask;
using task_scheduler_application.UseCases.ViewTasks;
using task_scheduler_application.UseCases.ViewNotifications;
using task_scheduler_application.DTO;

using task_scheduler_presentation.Views;
using task_scheduler_presentation.Models;
using task_scheduler_application.NotificationFrequencies;

namespace task_scheduler_presentation.Controllers {
    /// <summary>
    /// Offers and implements the functionality for a User of the Task-Scheduler application
    /// </summary>
    public class UserController {

        /// <summary>
        /// For creating new CreateTaskUseCases
        /// </summary>
        private readonly CreateTaskUseCaseFactory createTaskUseCaseFactory;

        /// <summary>
        /// For creating new ViewTaskUseCases
        /// </summary>
        private readonly ViewTasksUseCaseFactory viewTasksUseCaseFactory;

        /// <summary>
        /// for creating new ViewNotificationsUseCases
        /// </summary>
        private readonly ViewNotificationsUseCaseFactory viewNotificationsUseCaseFactory;

        public const string CUSTOM_NOTIFICATION_TYPE_STRING = "Custom";

        //TODO : abstract these strings out of the presentation layer
        /// <summary>
        /// The types of Notification Frequencies available in the application
        /// </summary>
        private readonly Dictionary<NotificationFrequencyType, string> frequenctTypeStrMap =
            new Dictionary<NotificationFrequencyType, string>() {
                { NotificationFrequencyType.Daily, "Daily" },
                { NotificationFrequencyType.AlternateDay, "Every Other Day" },
                { NotificationFrequencyType.Review, "Review" },
                { NotificationFrequencyType.Custom, CUSTOM_NOTIFICATION_TYPE_STRING }
            };

        public List<string> FrequencyTypeStrings {
            get { return frequenctTypeStrMap.Values.ToList(); }
        }


        /// <summary>
        /// Event for when a new TaskItem is created in the application
        /// </summary>
        public event EventHandler<TaskItemModel> TaskCreated;

        /// <summary>
        /// Invoked when a new Notification is created within the application
        /// </summary>
        public event EventHandler<NotificationModel> NotificationCreated;

        /// <summary>
        /// Executed whenever a new TaskItem is created. Invokes the delegates attached to the
        /// TaskCreated event.
        /// </summary>
        /// <param name="taskItem">
        /// A newly created TaskItem
        /// </param>
        protected void OnTaskCreated(TaskItemModel taskItem) {
            TaskCreated?.Invoke(this, taskItem);
        }

        /// <summary>
        /// Invokes the NotificationCreated event subscribers.
        /// </summary>
        /// <param name="notification">
        /// a newly created Notification
        /// </param>
        protected void OnNotificationCreated(NotificationModel notification) {
            NotificationCreated?.Invoke(this, notification);
        }

        public UserController(
            CreateTaskUseCaseFactory createTaskUseCaseFactory,
            ViewTasksUseCaseFactory viewTasksUseCaseFactory,
            ViewNotificationsUseCaseFactory viewNotificationsUseCaseFactory) {

            this.createTaskUseCaseFactory = createTaskUseCaseFactory ?? throw new ArgumentNullException(nameof(createTaskUseCaseFactory));
            this.viewTasksUseCaseFactory = viewTasksUseCaseFactory ?? throw new ArgumentNullException(nameof(viewTasksUseCaseFactory));
            this.viewNotificationsUseCaseFactory = viewNotificationsUseCaseFactory ?? throw new ArgumentNullException(nameof(viewNotificationsUseCaseFactory));
        }


        /// <summary>
        /// Retrieves Notifications in the application then adds them to an INotificationView's
        /// collection of Notifications to displays, also subscribes the view to the
        /// NotificationCreated event
        /// </summary>
        /// <param name="view">
        /// Will be given Notitications displayed and subscribed to the NotificationCreated Event
        /// </param>
        public void ViewNotifications(INotificationsView view) {

            //call use-case factory to create use-case object

            //execute the use-case

            //add Notifications from the use-cases output to the view's collection

            //subscribe view to NotificationCreated event
            NotificationCreated += view.NotificationCreatedCallback;

            //use the view's Closing event to unsubscribe it from NotificationCreated
            view.Closing += (s, e) => { NotificationCreated -= view.NotificationCreatedCallback; }; 
        }

        /// <summary>
        /// Retrieves the TaskItems in the application and gives them to a ITasksView to display.
        /// </summary>
        /// <param name="view">
        /// Displays TaskItems in the application
        /// </param>
        public void ViewTasks(ITasksView view) {

            var uc = viewTasksUseCaseFactory.New();

            uc.Execute();

            //add taskItemDTOs from UseCase output to observable collection for view
            foreach(TaskItemDTO taskItemDTO in uc.Output.TaskItems) {

                TaskItemModel taskItemModel = new TaskItemModel() {
                    Title = taskItemDTO.Title,
                    Description = taskItemDTO.Description,
                    FrequencyType = frequenctTypeStrMap[taskItemDTO.NotificationFrequencyType],
                    NotificationFrequency = taskItemDTO.CustomNotificationFrequency,
                    StartTime = taskItemDTO.StartTime,
                    Color = new Windows.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, taskItemDTO.R, taskItemDTO.G, taskItemDTO.B))
                };

                view.TaskItems.Add(taskItemModel);
            }

            //subscribe the view to our TaskCreated event
            TaskCreated += view.TaskCreatedCallback;

            //Subscribe to the views Closing event, so we can unsubscribe the 
            //view from the TaskCreated event
            view.Closing += (s, e) => { TaskCreated -= view.TaskCreatedCallback; };
        }


        /// <summary>
        /// Extracts TaskItem input data from a IAddTaskView and creates a new TaskItem for the
        /// application.
        /// </summary>
        /// <param name="view">
        /// Where new TaskItem info was inputted
        /// </param>
        public void CreateTask(IAddTaskView view) {

            //get input from view and create use-case input
            CreateTaskInput input = new CreateTaskInput {
                Title = view.Title,
                Description = view.Description,
                StartTime = view.StartTime,
                R = view.Color.R,
                G = view.Color.G,
                B = view.Color.B,
                NotificationFrequencyType = frequenctTypeStrMap.Where(
                    (x)=> { return x.Value == view.FrequencyType; }
                ).First().Key,
                CustomNotificationFrequency = view.CustomFrequency
            };

            //create UseCase instance and assign input structure to its input port
            var uc = createTaskUseCaseFactory.New();
            uc.Input = input;

            //run the use case
            uc.Execute();

            //get Use Case output and handle errors
            CreateTaskOutput output = uc.Output;

            if (output.Success) {
                //create task item model from taskItemDTO in successful use-case output
                TaskItemModel newTaskItemModel = new TaskItemModel() {
                    Title = output.TaskItemDTO.Title,
                    Description = output.TaskItemDTO.Description,
                    StartTime = output.TaskItemDTO.StartTime,
                    FrequencyType = 
                        frequenctTypeStrMap[output.TaskItemDTO.NotificationFrequencyType],
                    NotificationFrequency = output.TaskItemDTO.CustomNotificationFrequency,
                    Color = new Windows.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, output.TaskItemDTO.R, output.TaskItemDTO.G, output.TaskItemDTO.B))
                };

                //fire the TaskCreated event
                OnTaskCreated(newTaskItemModel);

                view.ClearFields();
                view.CloseSelf();
            }
            else {
                view.ApplicationErrorMessage = output.Error;
                view.ApplicationError = true;
            }
        }
    }
}
