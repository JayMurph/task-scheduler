using System;
using task_scheduler_entities;

namespace task_scheduler_application.NotificationFrequencies {
    /// <summary>
    /// Implements a User-defined NotificationFrequency
    /// </summary>
    public class CustomNotificationFrequency : IDescriptiveNotificationFrequency{

        /// <summary>
        /// Amount of time to elapse between Notifications
        /// </summary>
        private readonly TimeSpan period; 

        //TODO : abstract away magic string
        public string Description { get => "Custom"; }

        /// <summary>
        /// Creates a new CustomNotificationFrequency and assigns it a interval of time for which to
        /// produce Notifications
        /// </summary>
        /// <param name="period">How often to produce Notifications</param>
        public CustomNotificationFrequency(TimeSpan period) {
            this.period = period;
        }

        /// <summary>
        /// Produces the next point in time at which a Notification should be produced by a TaskItem, according to
        /// the NotificationFrequency
        /// </summary>
        /// <param name="taskStartTime">The point in time at which a TaskItem became active</param>
        /// <param name="now">The current time</param>
        /// <returns>An upcoming point in time at which a TaskItem should produce a Notification</returns>
        public DateTime NextNotificationTime(DateTime taskStartTime, DateTime now) {
            return NotificationFrequencyUtility.NextNotificationTime(taskStartTime, now, period);
        }

        /// <summary>
        /// Produces the length of time remaining until a TaskItem should produce its next
        /// Notification, according to the NotificationFrequency
        /// </summary>
        /// <param name="taskStartTime">The point in time at which a TaskItem became active</param>
        /// <param name="now">The current time</param>
        /// <returns>The time remaining until a TaskItem should produce its next Notification</returns>
        public TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now) {
            return NotificationFrequencyUtility.TimeUntilNextNotification(
                taskStartTime,
                now,
                NextNotificationTime
            );
        }
    }
}
