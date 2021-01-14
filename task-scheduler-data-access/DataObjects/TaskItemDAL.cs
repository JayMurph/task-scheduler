using System;

namespace task_scheduler_data_access_standard.DataObjects {

    /// <summary>
    /// behaviourless Data-layer representation of a TaskItem
    /// </summary>
    public class TaskItemDAL {

        /// <summary>
        /// Unique identifier for the TaskItem
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Title of the TaskItem
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Description of the TaskItem
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The time at which the TaskItem should begin generating notifications whenever its
        /// notification frequency elapses
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// The last point in time at which the TaskItem generated a Notification
        /// </summary>
        public DateTime LastNotificationTime { get; }

        /// <summary>
        /// R component of the TaskItem's RGB colour
        /// </summary>
        public byte R { get; }

        /// <summary>
        /// G component of the TaskItem's RGB colour
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// B component of the TaskItem's RGB colour
        /// </summary>
        public byte B { get; }

        public int NotificationFrequencyType { get; }

        /// <summary>
        /// A custom Notification Frequency that the TaskItem may use, instead of one the
        /// application defined Notification Frequencies
        /// </summary>
        public CustomNotificationFrequencyDAL CustomNotificationFrequency { get; } = null;

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

            Id = id;
            Title = title;
            Description = description;
            StartTime = startTime;
            LastNotificationTime = lastNotificationTime;
            R = r;
            G = g;
            B = b;
            CustomNotificationFrequency = customNotificationFrequency;
            NotificationFrequencyType = notificationFrequencyType;
        }
    }
}
