using System;

namespace task_scheduler_entities {
    /// <summary>
    /// The product of an ITaskItem when its notification time arrives
    /// </summary>
    public class Notification {

        #region Properties

        /// <summary>
        /// The TaskItem that produced the Notification
        /// </summary>
        public ITaskItem Producer { get; private set; }

        /// <summary>
        /// The time at which the Notification was produced
        /// </summary>
        public DateTime Time { get; private set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Notification. Assigns the incoming parameter's to the appropriate
        /// properties
        /// </summary>
        /// <param name="producer">
        /// The TaskItem that is generating the Notification
        /// </param>
        /// <param name="timeOfNotification">
        /// The time at which the notification is being generated
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The producer parameter was null
        /// </exception>
        public Notification( ITaskItem producer, DateTime timeOfNotification) {
            Producer = producer ?? throw new ArgumentNullException(nameof(producer));
            Time = timeOfNotification;
        }

        #endregion
    }
}
