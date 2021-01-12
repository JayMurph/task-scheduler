using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_data_access_standard.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.CreateTask {
    public class CreateTaskUseCaseFactory : IUseCaseFactory<CreateTaskUseCase> {

        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly IClock clock;
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public CreateTaskUseCaseFactory(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            IClock clock,
            ITaskItemRepositoryFactory taskItemRepositoryFactory){

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        public CreateTaskUseCase New() {
            return new CreateTaskUseCase(
                taskManager,
                notificationManager,
                clock,
                taskItemRepositoryFactory
            );
        }
    }
}
