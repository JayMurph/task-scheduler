using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access_standard.DataObjects {
    public class NotificationFrequencyDAL {
        public NotificationFrequencyDAL() {
        }

        public NotificationFrequencyDAL(Guid taskId, TimeSpan time) {
            Time = time;
        }


        public TimeSpan Time{ get; set; }
    }
}
