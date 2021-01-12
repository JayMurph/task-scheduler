using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access_standard.Repositories;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_application.UseCases.ViewTasks {
    public class ViewTasksUseCase : IUseCase<ViewTasksInput, ViewTasksOutput> {
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewTasksUseCase(
            ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        public ViewTasksInput Input { set; private get; }

        public ViewTasksOutput Output { get; private set; } = new ViewTasksOutput();

        public void Execute() {
            ITaskItemRepository taskRepo = taskItemRepositoryFactory.New();

            //go through all taskItems in database then add them to the Output
            //as TaskItemDTO's.
            foreach(TaskItemDAL taskDAL in taskRepo.GetAll()) {
                DTO.TaskItemDTO taskDTO =
                    new DTO.TaskItemDTO() {
                        Id = taskDAL.Id,
                        Title = taskDAL.Title,
                        Description = taskDAL.Description,
                        R = taskDAL.R,
                        G = taskDAL.G,
                        B = taskDAL.B,
                        StartTime = taskDAL.StartTime,
                        FrequencyType = taskDAL.NotificationFrequencyType,
                    };

                //if the current taskItem has a custom frequency type 
                //retrieve the custom time fromthe database
                //TODO: abstract out "Custom"
                if(taskDAL.CustomNotificationFrequency != null) {
                    taskDTO.CustomFrequency = taskDAL.CustomNotificationFrequency.Time;
                }

                Output.TaskItems.Add(taskDTO);
            }
        }
    }
}
