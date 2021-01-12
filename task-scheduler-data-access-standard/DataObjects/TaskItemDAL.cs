using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access_standard.DataObjects {

    public class TaskItemDAL {

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime LastNotificationTime { get; set; }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public string NotificationFrequencyType { get; set; }

        //public TimeSpan CustomNotificationFrequency { get; set; }

        public NotificationFrequencyDAL CustomNotificationFrequency { get; set; } = null;

        public List<NotificationDAL> Notifications { get; set; } = new List<NotificationDAL>();

        public TaskItemDAL() {
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
            NotificationFrequencyDAL  customNotificationFrequency) {

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
            CustomNotificationFrequency = customNotificationFrequency;
            NotificationFrequencyType = "Custom";
        }
    }
}
