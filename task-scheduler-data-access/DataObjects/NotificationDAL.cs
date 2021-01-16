using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access.DataObjects {
    /// <summary>
    /// Represents the data of a Notification produced by a TaskItem
    /// </summary>
    public class NotificationDAL {
        public NotificationDAL(Guid taskId, DateTime time) {
            TaskId = taskId;
            Time = time;
        }

        /// <summary>
        /// The unique id TaskItem that produced the notification
        /// </summary>
        public Guid TaskId { get; }

        /// <summary>
        /// The point in time at which the Notification was produced
        /// </summary>
        public DateTime Time { get; }
    }
}
