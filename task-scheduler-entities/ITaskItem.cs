using System;

namespace task_scheduler_entities {
    /// <summary>
    /// Interface for classes that are TaskItems that produce notifications
    /// </summary>
    public interface ITaskItem : IDisposable{

        /// <summary>
        /// The title of the ITaskItem
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Extra descriptive information for the ITaskItem
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// The time at which the ITaskItem was created
        /// </summary>
        DateTime StartTime { get; set; }
        DateTime LastNotificationTime { get; }
        bool IsActive { get; }

        /// <summary>
        /// Assigns a new INotificationPeriod to the ITaskItem to use for determining notification
        /// times
        /// </summary>
        /// <param name="period">
        /// To be assigned to the ITaskItem
        /// </param>
        void ChangePeriod(INotificationPeriod period);

        /// <summary>
        /// Stops the ITaskItem from producing anymore notifications for the remainder of its
        /// lifetime
        /// </summary>
        void Cancel();
    }
}