using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access.Repositories {
    /// <summary>
    /// Produces NotificationFrequencyRepositories 
    /// </summary>
    public class NotificationFrequencyRepositoryFactory : INotificationFrequencyRepositoryFactory {
        private readonly string connectionStr;

        public NotificationFrequencyRepositoryFactory(string connectionStr) {
            this.connectionStr = connectionStr;
        }

        public INotificationFrequencyRepository New() {
            return new NotificationFrequencyRepository(connectionStr);
        }
    }
}
