using System;

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// Defines the necessary components for a page that allows creating a new TaskItem
    /// </summary>
    public interface IAddTaskView {

        /// <summary>
        /// Title field for a new TaskItem
        /// </summary>
        string Title { get; } 

        /// <summary>
        /// Description field for a new TaskItem
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The desired activation time for the new TaskItem
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Color for the new TaskItem
        /// </summary>
        Windows.UI.Color Color { get; }

        /// <summary>
        /// The type of notification frequency for the new TaskItem to have
        /// </summary>
        string FrequencyType { get; }

        /// <summary>
        /// Custom length of time to use as the notification frequency of the new TaskItem
        /// </summary>
        TimeSpan CustomFrequency { get; }

        /// <summary>
        /// Holds a description of any errors that may have occured during the creation of a new
        /// TaskItem in the application layer
        /// </summary>
        string ApplicationErrorMessage { set; }

        /// <summary>
        /// Signals to the view if Errors occured below the application layer
        /// </summary>
        bool ApplicationError { set; }

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
