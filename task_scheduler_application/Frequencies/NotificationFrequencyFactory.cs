using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.Frequencies {
    public static class NotificationFrequencyFactory {
        public static IDescriptiveNotificationFrequency New(string type, TimeSpan customPeriod = new TimeSpan()) {
            switch (type) {
                //TODO : abstract away magic strings
                case "Daily": return new DailyNotificationFrequency();
                case "Every Other Day": return new BiDailyNotificationFrequency();
                case "Review": return new ReviewNotificationFrequency();
                case "Custom": return new CustomNotificationFrequency(customPeriod);
                default: return null;
            }
        }
    }
}
