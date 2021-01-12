using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.NotificationFrequencies {
    public static class NotificationFrequencyUtility {
        public static DateTime NextNotificationTime(DateTime taskStartTime, DateTime now, TimeSpan period) {
            TimeSpan periodAccum = period;

            while(taskStartTime + periodAccum <= now) {
                periodAccum = periodAccum.Add(period);
            }

            return taskStartTime + periodAccum;
        }

        public static TimeSpan TimeUntilNextNotification(
            DateTime taskStartTime,
            DateTime now,
            Func<DateTime, DateTime, DateTime> NextNotificationTimeProc) {

            return now.Subtract(NextNotificationTimeProc(taskStartTime, now));
        }
    }
}
