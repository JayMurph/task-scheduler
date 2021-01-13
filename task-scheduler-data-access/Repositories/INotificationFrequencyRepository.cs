using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_data_access_standard.Repositories {
    public interface INotificationFrequencyRepository : IRepository<NotificationFrequencyDAL> {
        NotificationFrequencyDAL GetById(Guid id);
        bool Delete(Guid id);
    }
}
