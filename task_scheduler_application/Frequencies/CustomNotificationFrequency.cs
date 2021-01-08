using System;
using task_scheduler_entities;

namespace task_scheduler_application.Frequencies {
    public class CustomNotificationFrequency : IDescriptiveNotificationFrequency{

        private readonly TimeSpan period; 

        //TODO : abstract away magic string
        public string Description { get => "Custom"; }

        public CustomNotificationFrequency(TimeSpan period) {
            this.period = period;
        }

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
