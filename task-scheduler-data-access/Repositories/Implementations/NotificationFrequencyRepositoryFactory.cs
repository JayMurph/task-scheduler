namespace task_scheduler_data_access.Repositories {
    /// <summary>
    /// Produces NotificationFrequencyRepositories 
    /// </summary>
    public class NotificationFrequencyRepositoryFactory : INotificationFrequencyRepositoryFactory {
        private readonly string connectionStr;

        public NotificationFrequencyRepositoryFactory(string connectionStr) {
            this.connectionStr = connectionStr;

            DataAccess.InitializeDatabase(connectionStr);
        }

        public INotificationFrequencyRepository New() {
            return new NotificationFrequencyRepository(connectionStr);
        }
    }
}
