using System;

namespace task_scheduler_data_access.DataObjects {
    /// <summary>
    /// Represents the data of a Notification produced by a TaskItem
    /// </summary>
    public struct NotificationDAL {
        public NotificationDAL(Guid taskId, DateTime time) {
            this.taskId = taskId;
            this.time = time;
        }

        /// <summary>
        /// The unique id TaskItem that produced the notification
        /// </summary>
        public readonly Guid taskId;

        /// <summary>
        /// The point in time at which the Notification was produced
        /// </summary>
        public readonly DateTime time; 
    }
}
