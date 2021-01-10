using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.NotificationFrequencies {
    public class DailyNotificationFrequency : IDescriptiveNotificationFrequency {
        private readonly TimeSpan period = new TimeSpan(1, 0, 0, 0);

        //TODO : abstract away magic string
        public string Description { get => "Daily"; }

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
