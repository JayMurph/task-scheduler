using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access_standard.Repositories;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_application.UseCases.ViewTasks {
    /// <summary>
    /// Encapsulates and executes the logic to retrieve TaskItems for viewing in the Task Scheduler
    /// application
    /// </summary>
    public class ViewTasksUseCase : IUseCase<ViewTasksInput, ViewTasksOutput> {
        /// <summary>
        /// Produces TaskItemRepository's which allow for retrieving tasks from a database
        /// </summary>
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewTasksInput Input { set; private get; }

        public ViewTasksOutput Output { get; private set; } = new ViewTasksOutput();

        /// <summary>
        /// Constructs a new ViewTasksUseCase
        /// </summary>
        /// <param name="taskItemRepositoryFactory">
        /// Produces <see cref="TaskItemRepository"/>'s allowing the <see cref="ViewTasksUseCase"/> to retrieve
        /// TaskItem data from a database
        /// </param>
        public ViewTasksUseCase(ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        /// <summary>
        /// Executes the logic of the <see cref="ViewTasksUseCase"/>. Retrieves TaskItem data and
        /// stores it in the <see cref="ViewTasksUseCase"/>'s Output property.
        /// </summary>
        public void Execute() {
            ITaskItemRepository taskRepo = taskItemRepositoryFactory.New();

            /*
             * go through all taskItems in database then add them to the Output property as
             * TaskItemDTO's.
             */
            foreach(TaskItemDAL taskData in taskRepo.GetAll()) {

                TimeSpan customFrequencyTime = new TimeSpan();

                //if the current taskItem has a custom frequency type, then retrieve its Time value
                if(taskData.CustomNotificationFrequency != null) {
                    customFrequencyTime = taskData.CustomNotificationFrequency.Time;
                }

                DTO.TaskItemDTO taskDTO =
                    new DTO.TaskItemDTO() {
                        Id = taskData.Id,
                        Title = taskData.Title,
                        Description = taskData.Description,
                        R = taskData.R,
                        G = taskData.G,
                        B = taskData.B,
                        StartTime = taskData.StartTime,
                        FrequencyType = taskData.NotificationFrequencyType,
                        CustomFrequency = customFrequencyTime
                    };

                Output.TaskItems.Add(taskDTO);
            }
        }
    }
}
