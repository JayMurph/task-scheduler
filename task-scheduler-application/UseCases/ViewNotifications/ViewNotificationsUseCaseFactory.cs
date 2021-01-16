using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access.Repositories;

namespace task_scheduler_application.UseCases.ViewNotifications {
    public class ViewNotificationsUseCaseFactory : IUseCaseFactory<ViewNotificationsUseCase> {
        INotificationManager notificationManager;
        ITaskItemRepositoryFactory taskItemRepositoryFactory;

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
