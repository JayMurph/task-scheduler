using System;
using task_scheduler_entities;

namespace task_scheduler {
    public class RealTimeClock : IClock {
        public object Clone() {
            return new RealTimeClock();
        }

        public DateTime Now {
            get { return DateTime.Now; }
        }
    }
}
