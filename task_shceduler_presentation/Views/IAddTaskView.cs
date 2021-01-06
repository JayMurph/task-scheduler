using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace task_scheduler_presentation.Views {
    public interface IAddTaskView {
        string Title { get; set; } 
        string Description { get; set; }
        DateTime StartTime { get; set; }
        Windows.UI.Color Color { get; set; }
        string FrequencyType { get; set; }
        TimeSpan CustomFrequency { get; set; }
        string Error { get; set; }
        void CloseSelf();
        void ClearFields();
    }

}
