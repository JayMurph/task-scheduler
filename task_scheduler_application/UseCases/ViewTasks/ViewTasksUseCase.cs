using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access_standard.Repositories;

namespace task_scheduler_application.UseCases.ViewTasks {
    public class ViewTasksUseCase : IUseCase<ViewTasksInput, ViewTasksOutput> {
        ITaskManager taskManager;
        ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewTasksUseCase(ITaskManager taskManager, ITaskItemRepositoryFactory taskItemRepositoryFactory) {
            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        public ViewTasksInput Input { set; private get; }

        public ViewTasksOutput Output { get; private set; } = new ViewTasksOutput();

        public void Execute() {
            //is there input data required for this?
            //possibly in the future

            var taskItems = taskManager.GetAll();

            foreach(TaskItem task in taskItems) {
                Output.TaskItems.Add(
                    new DTO.TaskItemDTO() {
                        ID = task.ID,
                        Title= task.Title,
                        Description = task.Description,
                        R = task.Colour.R,
                        G = task.Colour.G,
                        B = task.Colour.B,
                        StartTime = task.StartTime
                        //how to handle frequencies . . .
                    }
                );
            }
        }
    }
}
