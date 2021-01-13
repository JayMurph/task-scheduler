using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler_application.DTO;

namespace task_scheduler_presentation.Models{
    /// <summary>
    /// Represents a TaskItem so it can be displayed in the presentation layer
    /// </summary>
    public class TaskItemModel {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public string FrequencyType { get; set; }
        public TimeSpan NotificationFrequency { get; set; }
        public Windows.UI.Xaml.Media.Brush Color { get; set; }
    }
}
