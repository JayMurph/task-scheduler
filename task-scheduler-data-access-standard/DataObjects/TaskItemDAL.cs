using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access_standard.DataObjects {

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
