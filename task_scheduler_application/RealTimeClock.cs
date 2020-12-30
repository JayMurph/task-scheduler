using System;
using task_scheduler_entities;

namespace task_scheduler_application {
    public class RealTimeClock : IClock {
        public object Clone() {
            return new RealTimeClock();
        }

        public DateTime Now {
            get { return DateTime.Now; }
        }
    }
}
