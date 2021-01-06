﻿using System;
using task_scheduler_entities;

namespace task_scheduler_application.Frequencies {
    public class ConstantFrequency : INotificationFrequency {
        private readonly TimeSpan period; 

        public ConstantFrequency(TimeSpan period) {
            this.period = period;
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