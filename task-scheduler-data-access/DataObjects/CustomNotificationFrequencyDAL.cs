using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access_standard.DataObjects {
    /// <summary>
    /// Represents the data of a Custom Notification Frequency
    /// </summary>
    public class CustomNotificationFrequencyDAL {
        public CustomNotificationFrequencyDAL(Guid taskId, TimeSpan time) {
            TaskId = taskId;
            Time = time;
        }

        /// <summary>
        /// The unique id of the TaskItem that uses the Custom Notification Frequency
        /// </summary>
        public Guid TaskId { get; }

        /// <summary>
        /// The time interval that dictates when the custom Notification Frequency produces
        /// Notifications
        /// </summary>
        public TimeSpan Time { get; }
    }
}
