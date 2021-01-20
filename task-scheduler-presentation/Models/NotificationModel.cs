using System;

namespace task_scheduler_presentation.Models {
    /// <summary>
    /// Centralizes the contents of a Notification that will be presented to the user
    /// </summary>
    public class NotificationModel {
        /// <summary>
        /// The Title of the Task that generated the Notification
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The time at which the Notification was generated
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Color of the Task that generated the Notification
        /// </summary>
        public Windows.UI.Xaml.Media.Brush Color { get; set; }
    }
}
