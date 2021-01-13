using System;
using System.Collections.ObjectModel;
using task_scheduler_presentation.Models;

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// Defines the necessary components for a Page that can display TaskItems
    /// </summary>
    public interface ITasksView {
        /// <summary>
        /// Collection of TaskItemModels that the page will display
        /// </summary>
        ObservableCollection<TaskItemModel> TaskItems { get; set; }

        /// <summary>
        /// Callback to be invoked whenever a new TaskItemModel is to be added to the Page's
        /// collection to be displayed.
        /// </summary>
        /// <param name="source">
        /// The initiator of the event
        /// </param>
        /// <param name="taskItemModel">
        /// A newly created TaskItemModel. To be added to the TasksView page's collection to be
        /// displayed.
        /// </param>
        void TaskCreatedCallback(object source, TaskItemModel taskItemModel);

        /// <summary>
        /// Event that should be invoked when the TasksView page is closed
        /// </summary>
        event EventHandler Closing;
    }
}
