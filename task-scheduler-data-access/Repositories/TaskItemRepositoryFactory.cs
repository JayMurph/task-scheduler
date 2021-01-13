using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access_standard.Repositories {
    public class TaskItemRepositoryFactory : ITaskItemRepositoryFactory {
        private readonly string connectionStr;
        private readonly INotificationFrequencyRepositoryFactory notificationFrequencyRepositoryFactory;

        public TaskItemRepositoryFactory(string connectionStr, INotificationFrequencyRepositoryFactory notificationFrequencyRepositoryFactory) {
            this.connectionStr = connectionStr;
            this.notificationFrequencyRepositoryFactory = 
                notificationFrequencyRepositoryFactory ?? throw new ArgumentNullException(nameof(notificationFrequencyRepositoryFactory));
        }

        public ITaskItemRepository New() {
            return new TaskItemRepository(connectionStr, notificationFrequencyRepositoryFactory);
        }
    }
}
