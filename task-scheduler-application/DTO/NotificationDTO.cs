using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.DTO {
    /// <summary>
    /// Encapsulates the essential data of a Notification
    /// </summary>
    public struct NotificationDTO {
        /// <summary>
        /// Identifies the TaskItem that produced the Notification
        /// </summary>
        Guid TaskId { get; set; }

        /// <summary>
        /// The point in time at which the Notification was produced
        /// </summary>
        DateTime Time { get; set; }
    }
}
