using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
