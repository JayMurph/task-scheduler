using System;
using task_scheduler_application.NotificationFrequencies;
using task_scheduler_data_access.DataObjects;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;
using task_scheduler_utility;

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
        private readonly BasicTaskManager taskManager;

        /// <summary>
        /// Constructs a new ViewTasksUseCase
        /// </summary>
        public ViewTasksUseCase(BasicTaskManager taskManager, ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.taskManager = taskManager;
            this.taskItemRepositoryFactory = taskItemRepositoryFactory;
        }

        /// <summary>
        /// Executes the logic of the <see cref="ViewTasksUseCase"/>. Retrieves TaskItem data and
        /// stores it in the <see cref="ViewTasksUseCase"/>'s Output property.
        /// </summary>
        public ViewTasksOutput Execute(ViewTasksInput input) {
            ITaskItemRepository taskRepo = taskItemRepositoryFactory.New();

            ViewTasksOutput output = new ViewTasksOutput {
                Success = true
            };

            /*
             * go through all taskItems in database then add them to the Output property as
             * TaskItemDTO's.
             */
            foreach (TaskItem domainTask in taskManager.GetAll()) {

                //retrieve dataLayer task which carries notification frequency description
                Maybe<TaskItemDAL> maybeTask = taskRepo.GetById(domainTask.ID);

                if(maybeTask.HasValue) {
                    TaskItemDAL taskDAL = maybeTask.Value;
                    TimeSpan customFrequencyTime = new TimeSpan();

                    //if the current taskItem has a custom frequency type, then retrieve its Time value
                    if(taskDAL.customNotificationFrequency.HasValue) {
                        CustomNotificationFrequencyDAL frequencyDAL = taskDAL.customNotificationFrequency.Value;
                        customFrequencyTime = frequencyDAL.time;
                    }

                    DTO.TaskItemDTO taskDTO =
                        new DTO.TaskItemDTO() {
                            Id = domainTask.ID,
                            Title = domainTask.Title,
                            Description = domainTask.Description,
                            R = domainTask.Colour.R,
                            G = domainTask.Colour.G,
                            B = domainTask.Colour.B,
                            StartTime = domainTask.StartTime,
                            //TODO: prefer to do a better conversion that just a cast to an enum
                            NotificationFrequencyType = 
                                (NotificationFrequencyType)taskDAL.notificationFrequencyType,
                            CustomNotificationFrequency = customFrequencyTime
                        };

                    output.TaskItems.Add(taskDTO);
                }

            }

            return output;
        }
    }
}
