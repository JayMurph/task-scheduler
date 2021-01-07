using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.Frequencies {
    class ReviewFrequency : IDescriptiveNotificationFrequency {
        private readonly TimeSpan startingPeriod = new TimeSpan(12, 0, 0);

        //TODO : abstract away magic string
        public string Description { get => "Review"; }

        public DateTime NextNotificationTime(DateTime taskStartTime, DateTime now) {
            TimeSpan periodAccum = startingPeriod;

            while(taskStartTime + periodAccum <= now) {
                periodAccum = periodAccum.Add(periodAccum);
            }

            return taskStartTime + periodAccum;
        }

        public TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now) {
            return FrequencyUtility.TimeUntilNextNotification(taskStartTime, now, NextNotificationTime);
        }
    }
}
