using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access {
    public class NotificationFrequencyDAL {

        public Guid NotificationFrequencyDALId{ get; set; }

        public TaskItemDAL TaskItem{get;set;}

        public TimeSpan Frequency { get; set; }
    }
}
