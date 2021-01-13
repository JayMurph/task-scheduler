using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// Defines the necessary components for a page that allows creating a new TaskItem
    /// </summary>
    public interface IAddTaskView {

        /// <summary>
        /// Title field for a new TaskItem
        /// </summary>
        string Title { get; set; } 

        /// <summary>
        /// Description field for a new TaskItem
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The desired activation time for the new TaskItem
        /// </summary>
        DateTime StartTime { get; set; }

        /// <summary>
        /// Color for the new TaskItem
        /// </summary>
        Windows.UI.Color Color { get; set; }

        /// <summary>
        /// The type of notification frequency for the new TaskItem to have
        /// </summary>
        string FrequencyType { get; set; }

        /// <summary>
        /// Custom length of time to use as the notification frequency of the new TaskItem
        /// </summary>
        TimeSpan CustomFrequency { get; set; }

        /// <summary>
        /// Holds a description of any errors that may have occured during the creation of a new
        /// TaskItem
        /// </summary>
        string Error { get; set; }

        /// <summary>
        /// Closes the AddTaskView page
        /// </summary>
        void CloseSelf();

        /// <summary>
        /// Clears and sets the page's input fields back to their default values
        /// </summary>
        void ClearFields();
    }

}
