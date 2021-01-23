using System;

namespace task_scheduler_data_access.Repositories {
    /// <summary>
    /// Produces <see cref="TaskItemRepository"/>s
    /// </summary>
    public class TaskItemRepositoryFactory : ITaskItemRepositoryFactory {

        private readonly string connectionStr;
        private readonly INotificationFrequencyRepositoryFactory notificationFrequencyRepositoryFactory;

        public TaskItemRepositoryFactory(string connectionStr, INotificationFrequencyRepositoryFactory notificationFrequencyRepositoryFactory) {
            this.connectionStr = connectionStr;

            DataAccess.InitializeDatabase(connectionStr);

            this.notificationFrequencyRepositoryFactory = 
                notificationFrequencyRepositoryFactory ?? throw new ArgumentNullException(nameof(notificationFrequencyRepositoryFactory));
        }

        public ITaskItemRepository New() {
            return new TaskItemRepository(connectionStr, notificationFrequencyRepositoryFactory);
        }
    }
}
