using System;

namespace task_scheduler_application.NotificationFrequencies {
    /// <summary>
    /// Contains functions that support common operations of NotificationFrequency classes
    /// </summary>
    public static class NotificationFrequencyUtility {
        /// <summary>
        /// Returns the point in time at which a TaskItem should produce its next Notification
        /// </summary>
        /// <param name="taskStartTime">The point in time at which a TaskItem became active</param>
        /// <param name="now">The current time</param>
        /// <param name="period">Amount of time to be wait between producing Notifications</param>
        /// <returns>An upcoming point in time at which a TaskItem should produce a Notification</returns>
        public static DateTime NextNotificationTime(DateTime taskStartTime, DateTime now, TimeSpan period) {
            TimeSpan periodAccum = period;

            while(taskStartTime + periodAccum <= now) {
                periodAccum = periodAccum.Add(period);
            }

            return taskStartTime + periodAccum;
        }

        /// <summary>
        /// Produces the length of time remaining until a TaskItem should produce its next
        /// Notification
        /// </summary>
        /// <param name="taskStartTime">The point in time at which a TaskItem became active</param>
        /// <param name="now">The current time</param>
        /// <param name="NextNotificationTimeProc">
        /// Calculates the next point in time at which a
        /// TaskItem should produce a Notification 
        /// </param>
        /// <returns>The time remaining until a TaskItem should produce its next Notification</returns>
        public static TimeSpan TimeUntilNextNotification(
            DateTime taskStartTime,
            DateTime now,
            Func<DateTime, DateTime, DateTime> NextNotificationTimeProc) {

            return now.Subtract(NextNotificationTimeProc(taskStartTime, now));
        }
    }
}
