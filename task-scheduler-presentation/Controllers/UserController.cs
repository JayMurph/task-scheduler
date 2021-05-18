using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using task_scheduler_application;
using task_scheduler_application.DTO;
using task_scheduler_application.NotificationFrequencies;
using task_scheduler_application.UseCases.CreateTask;
using task_scheduler_application.UseCases.DeleteTask;
using task_scheduler_application.UseCases.ViewNotifications;
using task_scheduler_application.UseCases.ViewTasks;
using task_scheduler_presentation.Models;
using task_scheduler_presentation.Views;
using Windows.UI.Xaml.Media;

namespace task_scheduler_presentation.Controllers {
    /// <summary>
    /// Offers and implements the functionality for a User of the Task-Scheduler application
    /// </summary>
    public class UserController {

        private readonly TaskSchedulerApplication taskSchedulerApplication;
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

        /// <summary>
        /// List of strings describing the different notification frequencies available in the
        /// application.
        /// </summary>
        public List<string> FrequencyTypeStrings {
            get { return frequenctTypeStrMap.Values.ToList(); }
        }


        /// <summary>
        /// Event for when a new TaskItem is created in the application
        /// </summary>
        public event EventHandler<TaskItemModel> TaskCreated;

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

        public UserController(TaskSchedulerApplication taskSchedulerApplication) {
            this.taskSchedulerApplication = taskSchedulerApplication ?? throw new ArgumentNullException(nameof(taskSchedulerApplication));
        }

        /// <summary>
        /// Retrieves the TaskItems in the application and gives them to a ITasksView to display.
        /// </summary>
        /// <param name="view">
        /// Displays TaskItems in the application
        /// </param>
        public void ViewTasks(ITasksView view) {

            var uc = taskSchedulerApplication.NewViewTasksUseCase();

            ViewTasksOutput output = uc.Execute(new ViewTasksInput());

            if (output.Success) {
                /*
                 * iterate through TaskItems returned by the use case, convert them to models, then
                 * add them to the view to display
                 */
                foreach(TaskItemDTO taskDTO in output.TaskItems) {

                    //map the current taskDTOs NotificationFrequencyType enum to a string
                    string frequencyTypeStr = frequenctTypeStrMap[taskDTO.NotificationFrequencyType];

                    //convert the taskDTO's rgb color to a Windows brush
                    SolidColorBrush colorBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, taskDTO.R, taskDTO.G, taskDTO.B));

                    //create TaskItemModel from data of TaskItemDTO
                    TaskItemModel taskItemModel = new TaskItemModel() {
                        Id = taskDTO.Id,
                        Title = taskDTO.Title,
                        Description = taskDTO.Description,
                        FrequencyType = frequencyTypeStr,
                        NotificationFrequency = taskDTO.CustomNotificationFrequency,
                        StartTime = taskDTO.StartTime,
                        Color = colorBrush
                    };

                    view.TaskItems.Add(taskItemModel);
                }

                //subscribe the view to our TaskCreated event
                TaskCreated += view.TaskCreatedCallback;

                //Subscribe to the views Closing event, so we can unsubscribe the 
                //view from the TaskCreated event
                view.Closing += (s, e) => { TaskCreated -= view.TaskCreatedCallback; };
            }
            else {
                //TODO: handle possible failure of use-case
            }
        }


        /// <summary>
        /// Extracts TaskItem input data from a IAddTaskView and creates a new TaskItem for the
        /// application.
        /// </summary>
        /// <param name="view">
        /// Where new TaskItem info was inputted
        /// </param>
        public void CreateTask(IAddTaskView view) {

            //map the view's notification frequency type string to an enum value
            NotificationFrequencyType notificationFrequencyType =
                frequenctTypeStrMap.Where(x => x.Value == view.FrequencyType).First().Key;

            //create input for use-case from data of the view
            CreateTaskInput input = new CreateTaskInput {
                Title = view.Title,
                Description = view.Description,
                StartTime = view.StartTime,
                R = view.Color.R,
                G = view.Color.G,
                B = view.Color.B,
                NotificationFrequencyType = notificationFrequencyType,
                CustomNotificationFrequency = view.CustomFrequency
            };

            //create UseCase instance and assign input structure to its input port
            var uc = taskSchedulerApplication.NewCreateTaskUseCase();
            //run the use case
            

            CreateTaskOutput output = uc.Execute(input);

            if (output.Success) {
                //get the taskItemDTO returned creatd by the executed usecase
                TaskItemDTO taskDTO = output.TaskItemDTO;

                //get a string representation of the taskDTO's notification frequency type
                string frequencyTypeStr = frequenctTypeStrMap[taskDTO.NotificationFrequencyType];

                //convert the taskDTO's rgb color to a Windows brush
                SolidColorBrush colorBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, taskDTO.R, taskDTO.G, taskDTO.B));

                //create task item model from taskDTO
                TaskItemModel newTaskItemModel = new TaskItemModel() {
                    Id = taskDTO.Id,
                    Title = taskDTO.Title,
                    Description = taskDTO.Description,
                    StartTime = taskDTO.StartTime,
                    FrequencyType = frequencyTypeStr,
                    NotificationFrequency = taskDTO.CustomNotificationFrequency,
                    Color = colorBrush
                };

                //fire the TaskCreated event
                OnTaskCreated(newTaskItemModel);

                //clear the views input fields and close it
                view.ClearFields();
                view.CloseSelf();
            }
            else {
                //transfer the use-cases error to the view
                view.ApplicationErrorMessage = output.Error;
                view.ApplicationError = true;
            }
        }

        /// <summary>
        /// Extracts the id of a Task to delete from the given IDeleteTaskView
        /// and deletes that Task from the application
        /// </summary>
        /// <param name="view">
        /// Contains field with the TaskItem to delete from the application
        /// </param>
        public void DeleteTask(IDeleteTaskView view) {
            if(view.ModelToDelete != null) {//if there is a model to delete
                //execute DeleteTaskUseCase on the ID of the model to delete
                DeleteTaskUseCase deleteTaskUseCase = taskSchedulerApplication.NewDeleteTaskUseCase();
                DeleteTaskUseCaseOutput output = deleteTaskUseCase.Execute(new DeleteTaskUseCaseInput() { Id = view.ModelToDelete.Id});

                if (output.Success) {
                    //remove deleted model from view
                    view.TaskItems.Remove(view.ModelToDelete);
                    view.ModelToDelete = null;
                }
                else {
                    //handle errors
                }
            }
        }
    }
}
