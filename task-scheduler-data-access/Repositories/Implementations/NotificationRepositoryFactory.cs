namespace task_scheduler_data_access.Repositories {
    public class NotificationRepositoryFactory : INotificationRepositoryFactory {
        private readonly string connectionString;

        public NotificationRepositoryFactory(string connectionString) {
            this.connectionString = connectionString;
            DataAccess.InitializeDatabase(connectionString);
        }

        public INotificationRepository New() {
            return new NotificationRepository(connectionString);
        }
    }
}
