using System;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.ViewNotifications {
    public class ViewNotificationsUseCaseFactory : IUseCaseFactory<ViewNotificationsUseCase> {

        //dependencies required for ViewNotificaitonsUseCase
        private readonly INotificationRepositoryFactory notificationRepositoryFactory;
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewNotificationsUseCaseFactory(
            ITaskItemRepositoryFactory taskItemRepositoryFactory, 
            INotificationRepositoryFactory notificationRepositoryFactory) {

            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.notificationRepositoryFactory = notificationRepositoryFactory ?? throw new ArgumentNullException(nameof(notificationRepositoryFactory));
        }

        public ViewNotificationsUseCase New() {
            return new ViewNotificationsUseCase(notificationRepositoryFactory, taskItemRepositoryFactory);
        }
    }
}
