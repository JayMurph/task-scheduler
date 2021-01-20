using System;
using System.Collections.ObjectModel;
using task_scheduler_presentation.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// Page that displays TaskItems
    /// </summary>
    public sealed partial class TaskPage : Page, ITasksView{
        /// <summary>
        /// Collection of TaskItemModels that will be displayed on the page
        /// </summary>
        public ObservableCollection<TaskItemModel> TaskItems { get; set; } = new ObservableCollection<TaskItemModel>();

        /// <summary>
        /// Creates and setups a new TaskPage
        /// </summary>
        public TaskPage() {
            this.InitializeComponent();

            //Call the ViewTasks Use-Case to setup the page
            App.UserController.ViewTasks(this);

            //should this be done by the controller as well ??
            this.taskListView.ItemsSource = TaskItems;
        }

        /// <summary>
        /// Occurs when the page is being navigated away from
        /// </summary>
        public event EventHandler Closing;

        private void OnClosing(object source, EventArgs args) {
            Closing?.Invoke(source, args);
        }

        /// <summary>
        /// Takes a TaskItemModel and inserts it into the page's collection of TaskItems that are
        /// being displayed. Allows the ViewTasks Use-Case to update the TaskItems being displayed.
        /// </summary>
        /// <param name="source">
        /// The source of the event. Unused.
        /// </param>
        /// <param name="taskItemModel">
        /// To be added to the page's collection of TaskItemModels
        /// </param>
        public void TaskCreatedCallback(object source, TaskItemModel taskItemModel) {
            TaskItems.Add(taskItemModel);
        }

        /// <summary>
        /// Occurs when the page is being navigated away from. Invokes the OnClosing event.
        /// </summary>
        /// <param name="e">
        /// Arguments for the Navigating event. unused.
        /// </param>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            OnClosing(this, null);
            base.OnNavigatingFrom(e);
        }
    }
}
