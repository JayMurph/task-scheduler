using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access_core.DataObjects {
    public class NotificationFrequencyDAL {

        public Guid TaskId{ get; set; }

        public TimeSpan Time{ get; set; }
    }
}
