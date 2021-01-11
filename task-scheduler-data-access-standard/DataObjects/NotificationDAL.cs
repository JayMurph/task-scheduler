using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access_standard.DataObjects {
    public class NotificationDAL {
        public NotificationDAL(Guid taskId, DateTime time) {
            TaskId = taskId;
            Time = time;
        }

        public Guid TaskId { get; }
        public DateTime Time { get; }
    }
}
