using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.ViewTasks {
    public class ViewTasksUseCaseFactory : IUseCaseFactory<ViewTasksUseCase> {
        ITaskManager taskManager;

        public ViewTasksUseCaseFactory(ITaskManager taskManager) {
            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
        }

        public ViewTasksUseCase New() {
            return new ViewTasksUseCase(taskManager);
        }
    }
}
