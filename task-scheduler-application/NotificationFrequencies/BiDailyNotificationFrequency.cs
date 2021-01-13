using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.NotificationFrequencies {
    /// <summary>
    /// Implements a NotificationFrequency that activates every 48 hours
    /// </summary>
    class BiDailyNotificationFrequency : IDescriptiveNotificationFrequency {
        /// <summary>
        /// Length of time between Notifications
        /// </summary>
        private readonly TimeSpan period = new TimeSpan(2, 0, 0, 0);

        //TODO : abstract away magic string
        public string Description { get => "Every Other Day"; }

        public DateTime NextNotificationTime(DateTime taskStartTime, DateTime now) {
            return NotificationFrequencyUtility.NextNotificationTime(taskStartTime, now, period);
        }

        public TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now) {
            return NotificationFrequencyUtility.TimeUntilNextNotification(
                taskStartTime,
                now,
                NextNotificationTime
            );
        }
    }
}
