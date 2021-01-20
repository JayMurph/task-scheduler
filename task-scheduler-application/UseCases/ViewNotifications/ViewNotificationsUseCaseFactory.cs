using System;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.ViewNotifications {
    public class ViewNotificationsUseCaseFactory : IUseCaseFactory<ViewNotificationsUseCase> {

        //dependencies required for ViewNotificaitonsUseCase
        private readonly INotificationManager notificationManager;
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewNotificationsUseCaseFactory(
            INotificationManager notificationManager,
            ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        public ViewNotificationsUseCase New() {
            return new ViewNotificationsUseCase(notificationManager, taskItemRepositoryFactory);
        }
    }
}
