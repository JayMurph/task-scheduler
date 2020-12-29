using System;

namespace task_scheduler_entities {
    /// <summary>
    /// Interface for classes that can calculate the time when a TaskItem should produce a
    /// notification
    /// </summary>
    public interface INotificationPeriod {

        /// <summary>
        /// Returns the time remaining before a TaskItem should produce a notification
        /// </summary>
        /// <param name="taskStartTime">
        /// The time at which a TaskTime was started
        /// </param>
        /// <param name="now">
        /// A point in time occuring after the taskStartTime
        /// </param>
        /// <returns>
        /// The time remaining before a TaskItem should produce a notification
        /// </returns>
        TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now);

        /// <summary>
        /// Returns the time at which a TaskItem should produce a notification
        /// </summary>
        /// <param name="taskStartTime">
        /// The time at which a TaskTime was started
        /// </param>
        /// <param name="now">
        /// A point in time occuring after the taskStartTime
        /// </param>
        /// <returns>
        /// The time at which a TaskItem should produce a notification
        /// </returns>
        DateTime NextNotificationTime(DateTime taskStartTime, DateTime now);
    }
}
