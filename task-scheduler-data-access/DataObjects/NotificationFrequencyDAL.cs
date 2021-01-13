using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access_standard.DataObjects {
    public class NotificationFrequencyDAL {
        public NotificationFrequencyDAL(Guid taskId, TimeSpan time) {
            TaskId = taskId;
            Time = time;
        }

        public Guid TaskId { get; }
        public TimeSpan Time { get; }
    }
}
