using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access {

    public class TaskItemDAL {

        public Guid TaskItemDALId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime {get;set;}
        public DateTime LastNotificationTime { get; set; }
             
        public byte R;
        public byte G;
        public byte B;
        
        public string FrequencyType;

        public NotificationFrequencyDAL NotificationFrequency { get; set; }
    }
}
