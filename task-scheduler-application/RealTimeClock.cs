using System;
using task_scheduler_entities;

namespace task_scheduler_application {
    /// <summary>
    /// A clock that produces the actual time
    /// </summary>
    public class RealTimeClock : IClock {
        public object Clone() {
            return new RealTimeClock();
        }
        public DateTime Now {
            get { return DateTime.Now; }
        }
    }
}
