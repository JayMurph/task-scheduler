using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.Frequencies;

namespace task_scheduler_application.DTO {
    public class TaskItemDTO {
        public Guid Id { get; set; }
        public string Title{ get; set; }
        public string Description{ get; set; }
        
        public DateTime StartTime{ get; set; }
        
        public byte R { get; set; }
        public byte G{get;set;}
        public byte B{get;set;}
        
        public string FrequencyType{get;set;}
        
        public TimeSpan CustomFrequency{get;set;}
    }
}
