using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access_standard.Repositories {
    public class FrequencyRepositoryFactory : IFrequencyRepositoryFactory {
        private readonly string connectionStr;

        public FrequencyRepositoryFactory(string connectionStr) {
            this.connectionStr = connectionStr;
        }

        public IFrequencyRepository New() {
            return new FrequencyRepository(connectionStr);
        }
    }
}
