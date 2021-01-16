using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access.Repositories;

namespace task_scheduler_application.UseCases.ViewNotifications {
    class ViewNotificationsUseCaseFactory : IUseCaseFactory<ViewNotificationsUseCase> {
        INotificationManager notificationManager;
        ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewNotificationsUseCaseFactory(INotificationManager notificationManager, ITaskItemRepositoryFactory taskItemRepositoryFactory) {
            this.notificationManager = notificationManager;
            this.taskItemRepositoryFactory = taskItemRepositoryFactory;
        }

        public ViewNotificationsUseCase New() {
            return new ViewNotificationsUseCase(notificationManager, taskItemRepositoryFactory);
        }
    }
}
