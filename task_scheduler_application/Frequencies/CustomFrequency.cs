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
            TimeSpan periodAccum = period;

            while(taskStartTime + periodAccum <= now) {
                periodAccum = periodAccum.Add(period);
            }

            return taskStartTime + periodAccum;
        }

        public TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now) {
            return now.Subtract(NextNotificationTime(taskStartTime, now));
        }
    }
}
