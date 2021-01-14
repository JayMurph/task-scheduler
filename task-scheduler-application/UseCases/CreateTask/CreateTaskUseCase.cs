using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_application.NotificationFrequencies;
using task_scheduler_application.DTO;
using task_scheduler_data_access_standard.Repositories;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_application.UseCases.CreateTask {
    /// <summary>
    /// Encapsulates the logic required for creating a new TaskItem
    /// </summary>
    public class CreateTaskUseCase : IUseCase<CreateTaskInput, CreateTaskOutput> {

        #region Properties
        /// <summary>
        /// Holds all currently active TaskItem's in the application. After the Use-Case creates a
        /// new TaskItem, it will give it to the taskManager.
        /// </summary>
        private readonly ITaskManager taskManager;

        /// <summary>
        /// Receives and maintains Notifications produced by TaskItem's. Required for the
        /// construction of a new TaskItem.
        /// </summary>
        private readonly INotificationManager notificationManager;

        /// <summary>
        /// Retrieves the current time. Required for the creation of a new TaskItem.
        /// </summary>
        private readonly IClock clock;

        /// <summary>
        /// Produces <see cref="TaskItemRepository"/>s, allowing the Use-Case to add newly created
        /// TaskItems to a database
        /// </summary>
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public CreateTaskInput Input { set; private get; } = null;

        public CreateTaskOutput Output { get; private set; } = null;

        #endregion 

        #region AddTaskUseCase Constructor

        public CreateTaskUseCase(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            IClock clock,
            ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        #endregion

        public void Execute() {
            //TODO: Validate input data within the Input property

            INotificationFrequency frequency = NotificationFrequencyFactory.New(Input.NotificationFrequencyType, Input.CustomNotificationFrequency);

            //create new Domain TaskItem from the supplied Input data
            TaskItem newTask = new TaskItem(
                Input.Title,
                Input.Description,
                new task_scheduler_entities.Colour(
                    Input.R, Input.G, Input.B
                ),
                Input.StartTime,
                notificationManager,
                frequency,
                clock
            );

            //add task to task manager, check for errors
            if (taskManager.Add(newTask)) {

                CustomNotificationFrequencyDAL notificationFrequency = null;

                //if the new task should have a custom notification frequency
                if (Input.NotificationFrequencyType == NotificationFrequencyType.Custom) {
                    //create a custom notification frequency to save to the database
                    notificationFrequency = new CustomNotificationFrequencyDAL(
                        newTask.ID,
                        Input.CustomNotificationFrequency
                    );
                }

                //Create a TaskItemDAL to save to the database
                TaskItemDAL taskItemDAL = new TaskItemDAL(
                    newTask.ID,
                    newTask.Title,
                    newTask.Description,
                    newTask.StartTime,
                    newTask.LastNotificationTime,
                    newTask.Colour.R,
                    newTask.Colour.G,
                    newTask.Colour.B,
                    notificationFrequency,
                    (int)Input.NotificationFrequencyType
                );

                ITaskItemRepository taskItemRepo = taskItemRepositoryFactory.New();

                //add and save the new TaskItemDAL to database
                //TODO: check for errors when adding and saving
                taskItemRepo.Add(taskItemDAL);
                taskItemRepo.Save();

                //create DTO to return as Output data
                TaskItemDTO taskItemDTO = new TaskItemDTO() {
                    Title = Input.Title,
                    Description = Input.Description,
                    StartTime = Input.StartTime,
                    CustomNotificationFrequency = Input.CustomNotificationFrequency,
                    NotificationFrequencyType = Input.NotificationFrequencyType,
                    R = Input.R,
                    G = Input.G,
                    B = Input.B
                };

                //fill output data and return
                Output = new CreateTaskOutput() { 
                    Success = true ,
                    TaskItemDTO = taskItemDTO
                };
            }
            else {
                //fill output data and return
                Output = new CreateTaskOutput() { Success = false, Error = "ERROR" };
            }
        }
    }
}
