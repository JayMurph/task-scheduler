using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_application.NotificationFrequencies;
using task_scheduler_application.DTO;
using task_scheduler_data_access_standard.Repositories;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_application.UseCases.CreateTask {
    public class CreateTaskUseCase : IUseCase<CreateTaskInput, CreateTaskOutput> {

        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly IClock clock;
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

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

        public CreateTaskInput Input { set; private get; } = null;

        public CreateTaskOutput Output { get; private set; } = null;

        public void Execute() {
            //validate input data

            IDescriptiveNotificationFrequency frequency = null;

            //create appropriate frequency for new TaskItem
            //TODO : abstract away magic string
            if(Input.FrequencyType == "Custom") {
                frequency = NotificationFrequencyFactory.New(Input.FrequencyType, Input.CustomFrequency);
            }
            else {
                frequency = NotificationFrequencyFactory.New(Input.FrequencyType);
            }

            //create new TaskItem from input data
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
                TaskItemDAL taskItemDAL =
                    new TaskItemDAL(
                        newTask.ID,
                        newTask.Title,
                        newTask.Description,
                        newTask.StartTime,
                        newTask.LastNotificationTime,
                        newTask.Colour.R,
                        newTask.Colour.G,
                        newTask.Colour.B,
                        frequency.Description
                    );

                //add task frequency to database if it is a custom frequency
                //TODO : abstract away magic string
                if(frequency.Description == "Custom") {
                    taskItemDAL.CustomNotificationFrequency = Input.CustomFrequency;
                }

                ITaskItemRepository taskItemRepository = taskItemRepositoryFactory.New();

                //add task to task repo, check for errors
                taskItemRepository.Add(taskItemDAL);

                taskItemRepository.Save();

                TaskItemDTO taskItemDTO = new TaskItemDTO() {
                    Title = Input.Title,
                    Description = Input.Description,
                    StartTime = Input.StartTime,
                    CustomFrequency = Input.CustomFrequency,
                    FrequencyType = Input.FrequencyType,
                    R = Input.R,
                    G = Input.G,
                    B = Input.B
                };

                //fill out output data and return
                Output = new CreateTaskOutput() { 
                    Success = true ,
                    TaskItemDTO = taskItemDTO
                };
            }
            else {
                //fill out output data and return
                Output = new CreateTaskOutput() { Success = false, Error = "ERROR" };
            }
        }
    }
}
