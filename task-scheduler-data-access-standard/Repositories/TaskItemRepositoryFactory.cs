using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access_standard.Repositories {
    public class TaskItemRepositoryFactory : ITaskItemRepositoryFactory {
        private readonly string connectionStr;

        public TaskItemRepositoryFactory(string connectionStr) {
            this.connectionStr = connectionStr;
        }

        public ITaskItemRepository New() {
            return new TaskItemRepository(connectionStr);
        }
    }
}
