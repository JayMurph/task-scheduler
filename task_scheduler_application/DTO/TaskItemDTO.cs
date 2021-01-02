using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.Frequencies;

namespace task_scheduler_application.DTO {
    public class TaskItemDTO {
        public Guid ID;
        public string Title;
        public string Description;
        
        public DateTime StartTime;
        
        public byte R;
        public byte G;
        public byte B;
        
        public FrequencyTypes FrequencyType;
        
        public TimeSpan CustomFrequency;
        
    }
}
