using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.Frequencies {
    public static class NotificationFrequencyFactory {
        public static IDescriptiveNotificationFrequency New(string type, TimeSpan customPeriod = new TimeSpan()) {
            switch (type) {
                //TODO : abstract away magic strings
                case "Daily": return new DailyFrequency();
                case "Every Other Day": return new EveryOtherDayFrequency();
                case "Review": return new ReviewFrequency();
                case "Custom": return new CustomFrequency(customPeriod);
                default: return null;
            }
        }
    }
}
