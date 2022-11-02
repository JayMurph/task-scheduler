using System;

namespace task_scheduler_data_access.DataObjects {
    /// <summary>
    /// Represents the data of a Custom Notification Frequency
    /// </summary>
    public struct CustomNotificationFrequencyDAL {
        public CustomNotificationFrequencyDAL(Guid taskId, TimeSpan time) {
            this.taskId = taskId;
            this.time = time;
        }

        /// <summary>
        /// The unique id of the TaskItem that uses the Custom Notification Frequency
        /// </summary>
        public Guid taskId;

        /// <summary>
        /// The time interval that dictates when the custom Notification Frequency produces
        /// Notifications
        /// </summary>
        public TimeSpan time; 
    }
}
