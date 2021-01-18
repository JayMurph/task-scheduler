using System;

namespace task_scheduler_data_access.DataObjects {

    /// <summary>
    /// behaviourless Data-layer representation of a TaskItem
    /// </summary>
    public struct TaskItemDAL {

        /// <summary>
        /// Unique identifier for the TaskItem
        /// </summary>
        public Guid id;

        /// <summary>
        /// Title of the TaskItem
        /// </summary>
        public string title;

        /// <summary>
        /// Description of the TaskItem
        /// </summary>
        public string description;

        /// <summary>
        /// The time at which the TaskItem should begin generating notifications whenever its
        /// notification frequency elapses
        /// </summary>
        public DateTime startTime;

        /// <summary>
        /// The last point in time at which the TaskItem generated a Notification
        /// </summary>
        public DateTime lastNotificationTime;

        /// <summary>
        /// R component of the TaskItem's RGB colour
        /// </summary>
        public byte r;

        /// <summary>
        /// G component of the TaskItem's RGB colour
        /// </summary>
        public byte g;

        /// <summary>
        /// B component of the TaskItem's RGB colour
        /// </summary>
        public byte b;

        public int notificationFrequencyType;

        /// <summary>
        /// A custom Notification Frequency that the TaskItem may use, instead of one the
        /// application defined Notification Frequencies
        /// </summary>
        public CustomNotificationFrequencyDAL customNotificationFrequency; 

        public TaskItemDAL(
            Guid id,
            string title,
            string description,
            DateTime startTime,
            DateTime lastNotificationTime,
            byte r,
            byte g,
            byte b,
            CustomNotificationFrequencyDAL customNotificationFrequency,
            int notificationFrequencyType) {

            if (string.IsNullOrWhiteSpace(title)) {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or empty", nameof(title));
            }

            if (description == null) {
                throw new ArgumentException($"'{nameof(description)}' cannot be null", nameof(description));
            }

            this.id = id;
            this.title = title;
            this.description = description;
            this.startTime = startTime;
            this.lastNotificationTime = lastNotificationTime;
            this.r = r;
            this.g = g;
            this.b = b;
            this.customNotificationFrequency = customNotificationFrequency;
            this.notificationFrequencyType = notificationFrequencyType;
        }
    }
}
