using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.Frequencies {
    class DailyFrequency : IDescriptiveNotificationFrequency {
        private readonly TimeSpan period = new TimeSpan(1, 0, 0, 0);
        public string Description { get => "Daily"; }

        public DateTime NextNotificationTime(DateTime taskStartTime, DateTime now) {
            throw new NotImplementedException();
        }

        public TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now) {
            throw new NotImplementedException();
        }
    }
}
