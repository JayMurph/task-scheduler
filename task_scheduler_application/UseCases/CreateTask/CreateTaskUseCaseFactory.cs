using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.CreateTask {
    public class CreateTaskUseCaseFactory : IUseCaseFactory<CreateTaskUseCase> {

        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly Repositories.ITaskItemRepository taskRepo;
        private readonly IClock clock;

        public CreateTaskUseCaseFactory(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            ITaskItemRepository taskRepo,
            IClock clock) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.taskRepo = taskRepo ?? throw new ArgumentNullException(nameof(taskRepo));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public CreateTaskUseCase New() {
            return new CreateTaskUseCase(
                taskManager,
                notificationManager,
                taskRepo,
                clock
            );
        }
    }
}
