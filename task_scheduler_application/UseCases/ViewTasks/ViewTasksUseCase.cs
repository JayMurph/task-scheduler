using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access_standard.Repositories;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_application.UseCases.ViewTasks {
    public class ViewTasksUseCase : IUseCase<ViewTasksInput, ViewTasksOutput> {
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;
        private readonly IFrequencyRepositoryFactory frequencyRepositoryFactory;

        public ViewTasksUseCase(
            ITaskItemRepositoryFactory taskItemRepositoryFactory,
            IFrequencyRepositoryFactory frequencyRepositoryFactory) {

            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.frequencyRepositoryFactory = frequencyRepositoryFactory ?? throw new ArgumentNullException(nameof(frequencyRepositoryFactory));
        }

        public ViewTasksInput Input { set; private get; }

        public ViewTasksOutput Output { get; private set; } = new ViewTasksOutput();

        public void Execute() {
            ITaskItemRepository taskRepo = taskItemRepositoryFactory.New();
            IFrequencyRepository freqRepo = frequencyRepositoryFactory.New();

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
                        FrequencyType = taskDAL.FrequencyType
                    };

                //if the current taskItem has a custom frequency type 
                //retrieve the custom time fromthe database
                //TODO: abstract out "Custom"
                if(taskDAL.FrequencyType == "Custom") {
                    NotificationFrequencyDAL frequencyDAL = freqRepo.GetById(taskDAL.Id);
                    taskDTO.CustomFrequency = frequencyDAL.Time;
                }

                Output.TaskItems.Add(taskDTO);
            }
        }
    }
}
