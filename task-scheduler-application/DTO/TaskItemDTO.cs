using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.NotificationFrequencies;

namespace task_scheduler_application.DTO {

    /// <summary>
    /// Encapsulates the essential data elements of a TaskItem
    /// </summary>
    public struct TaskItemDTO {
        public Guid Id { get; set; }
        public string Title{ get; set; }
        public string Description{ get; set; }
        
        public DateTime StartTime{ get; set; }
        
        public byte R { get; set; }
        public byte G{get;set;}
        public byte B{get;set;}
        
        /// <summary>
        /// Describes the type of NotificationFrequency being used by a TaskItem
        /// </summary>
        public NotificationFrequencyType NotificationFrequencyType{get;set;}
        
        /// <summary>
        /// Holds a non-pre-defined NotificationFrequency 
        /// </summary>
        public TimeSpan CustomNotificationFrequency{get;set;}
    }
}
