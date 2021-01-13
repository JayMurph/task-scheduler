using System;

namespace task_scheduler_data_access_standard.DataObjects {

    /// <summary>
    /// behaviourless Data-layer representation of a TaskItem
    /// </summary>
    public class TaskItemDAL {

        public Guid Id { get; }

        public string Title { get; }

        public string Description { get; }

        public DateTime StartTime { get; }
        public DateTime LastNotificationTime { get; }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public string NotificationFrequencyType { get; }

        public NotificationFrequencyDAL CustomNotificationFrequency { get; } = null;

        /// <summary>
        /// Constructs a TaskItemDAL which uses a pre-defined Notification Frequency
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="startTime"></param>
        /// <param name="lastNotificationTime"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="notificationFrequencyType"></param>
        public TaskItemDAL(
            Guid id,
            string title,
            string description,
            DateTime startTime,
            DateTime lastNotificationTime,
            byte r,
            byte g,
            byte b,
            string notificationFrequencyType) {

            if (string.IsNullOrEmpty(title)) {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or empty", nameof(title));
            }

            if (string.IsNullOrEmpty(description)) {
                throw new ArgumentException($"'{nameof(description)}' cannot be null", nameof(description));
            }

            if (string.IsNullOrEmpty(notificationFrequencyType)) {
                throw new ArgumentException($"'{nameof(notificationFrequencyType)}' cannot be null or empty", nameof(notificationFrequencyType));
            }

            Id = id;
            Title = title;
            Description = description;
            StartTime = startTime;
            LastNotificationTime = lastNotificationTime;
            R = r;
            G = g;
            B = b;
            NotificationFrequencyType = notificationFrequencyType;
        }

        /// <summary>
        /// Constructs a TaskItemDAL with a custom Notification Frequency
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="startTime"></param>
        /// <param name="lastNotificationTime"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="customNotificationFrequency"></param>
        public TaskItemDAL(
            Guid id,
            string title,
            string description,
            DateTime startTime,
            DateTime lastNotificationTime,
            byte r,
            byte g,
            byte b,
            NotificationFrequencyDAL customNotificationFrequency) {

            if (string.IsNullOrEmpty(title)) {
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
            CustomNotificationFrequency = customNotificationFrequency ?? throw new ArgumentNullException(nameof(customNotificationFrequency));
            NotificationFrequencyType = "Custom";
        }
    }
}
