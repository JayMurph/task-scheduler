using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.AddTask {
    public class AddTaskUseCaseFactory : IUseCaseFactory<AddTaskUseCase> {

        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly Repositories.ITaskItemRepository taskRepo;
        private readonly IClock clock;

        public AddTaskUseCaseFactory(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            ITaskItemRepository taskRepo,
            IClock clock) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.taskRepo = taskRepo ?? throw new ArgumentNullException(nameof(taskRepo));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public AddTaskUseCase New() {
            return new AddTaskUseCase(
                taskManager,
                notificationManager,
                taskRepo,
                clock
            );
        }
    }
}
