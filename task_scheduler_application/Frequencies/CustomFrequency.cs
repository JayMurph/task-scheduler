using System;
using task_scheduler_entities;

namespace task_scheduler_application.Frequencies {
    public class CustomFrequency : IDescriptiveNotificationFrequency{

        private readonly TimeSpan period; 
        public string Description { get; protected set; }

        public CustomFrequency(TimeSpan period) {
            this.period = period;

            //TODO : abstract away 'magic' string
            Description = "Custom";
        }

        public DateTime NextNotificationTime(DateTime taskStartTime, DateTime now) {
            return FrequencyUtility.NextNotificationTime(taskStartTime, now, period);
        }

        public TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now) {
            return FrequencyUtility.TimeUntilNextNotification(
                taskStartTime,
                now,
                NextNotificationTime
            );
        }
    }
}
