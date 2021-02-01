using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.DeleteTask {
    /// <summary>
    /// Produces DeleteTaskUseCase objects
    /// </summary>
    public class DeleteTaskUseCaseFactory : IUseCaseFactory<DeleteTaskUseCase> {

        //dependencies for DeleteTaskUseCases
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;
        private readonly INotificationRepositoryFactory notificationRepositoryFactory;
        private readonly BasicTaskManager taskItemManager;
        private readonly BasicNotificationManager notificationManager;

        public DeleteTaskUseCaseFactory(
            ITaskItemRepositoryFactory taskItemRepositoryFactory,
            INotificationRepositoryFactory notificationRepositoryFactory,
            BasicTaskManager taskItemManager,
            BasicNotificationManager notificationManager) {

            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.notificationRepositoryFactory = notificationRepositoryFactory ?? throw new ArgumentNullException(nameof(notificationRepositoryFactory));
            this.taskItemManager = taskItemManager ?? throw new ArgumentNullException(nameof(taskItemManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
        }

        /// <summary>
        /// Creates and returns a new DeleteTaskUseCase object
        /// </summary>
        /// <returns></returns>
        public DeleteTaskUseCase New() {
            return new DeleteTaskUseCase(taskItemRepositoryFactory, notificationRepositoryFactory, taskItemManager, notificationManager);
        }
    }
}
