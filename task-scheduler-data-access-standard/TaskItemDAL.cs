using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access_standard.DataObjects {

    public class TaskItemDAL {

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime {get;set;}
        public DateTime LastNotificationTime { get; set; }
             
        public byte R;
        public byte G;
        public byte B;
        
        public string FrequencyType;

        public TaskItemDAL() {
        }

        public TaskItemDAL(
            Guid id,
            string title,
            string description,
            DateTime startTime,
            DateTime lastNotificationTime,
            byte r,
            byte g,
            byte b,
            string frequencyType) {

            Id = id;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            StartTime = startTime;
            LastNotificationTime = lastNotificationTime;
            R = r;
            G = g;
            B = b;
            FrequencyType = frequencyType ?? throw new ArgumentNullException(nameof(frequencyType));
        }
    }
}
