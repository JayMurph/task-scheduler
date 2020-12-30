using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.AddTask {
    public class AddTaskUseCaseFactory : IUseCaseFactory<AddTaskUseCase> {

        ITaskManager taskManager;
        INotificationManager notificationManager;
        Repositories.ITaskItemRepository taskRepo;
        IClock clock;

        public AddTaskUseCaseFactory(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            ITaskItemRepository taskRepo,
            IClock clock) {

            this.taskManager = taskManager;
            this.notificationManager = notificationManager;
            this.taskRepo = taskRepo;
            this.clock = clock;
        }

        public AddTaskUseCase New() {
            throw new NotImplementedException();
            //return a new AddTaskUseCase
        }
    }
}
