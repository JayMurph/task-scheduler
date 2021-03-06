﻿using System;

namespace task_scheduler_application.DTO {
    /// <summary>
    /// Encapsulates the essential data of a Notification
    /// </summary>
    public struct NotificationDTO {
        /// <summary>
        /// Identifies the TaskItem that produced the Notification
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Title of the task that produced the Notification
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The point in time at which the Notification was produced
        /// </summary>
        public DateTime Time { get; set; }

        //RGB colour components
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }
}
