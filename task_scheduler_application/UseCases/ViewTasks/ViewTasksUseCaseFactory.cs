using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access_standard.Repositories;

namespace task_scheduler_application.UseCases.ViewTasks {
    public class ViewTasksUseCaseFactory : IUseCaseFactory<ViewTasksUseCase> {
        ITaskManager taskManager;
        ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewTasksUseCaseFactory(ITaskManager taskManager, ITaskItemRepositoryFactory taskItemRepositoryFactory) {
            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        public ViewTasksUseCase New() {
            return new ViewTasksUseCase(taskManager,taskItemRepositoryFactory);
        }
    }
}
