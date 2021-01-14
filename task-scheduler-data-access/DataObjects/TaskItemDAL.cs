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

        /// <summary>
        /// Describes the type of Notification Frequency that the TaskItem uses
        /// </summary>
        public string NotificationFrequencyType { get; }

        /// <summary>
        /// A custom Notification Frequency that the TaskItem may use, instead of one the
        /// application defined Notification Frequencies
        /// </summary>
        public CustomNotificationFrequencyDAL CustomNotificationFrequency { get; } = null;

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
        /// <param name="notificationFrequencyType">
        /// Describes a application defined Notification Frequency for the TaskItem to use
        /// </param>
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
        /// <param name="customNotificationFrequency">
        /// A non-application-defined Notification Frequency for the TaskItem to use
        /// </param>
        public TaskItemDAL(
            Guid id,
            string title,
            string description,
            DateTime startTime,
            DateTime lastNotificationTime,
            byte r,
            byte g,
            byte b,
            CustomNotificationFrequencyDAL customNotificationFrequency) {

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
            CustomNotificationFrequency = customNotificationFrequency ?? throw new ArgumentNullException(nameof(customNotificationFrequency));
            NotificationFrequencyType = "Custom";
        }
    }
}
