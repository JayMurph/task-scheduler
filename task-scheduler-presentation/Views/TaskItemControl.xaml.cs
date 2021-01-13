using Windows.UI.Xaml.Controls;
using task_scheduler_presentation.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace task_scheduler_presentation.Views {
    public sealed partial class TaskItemControl : UserControl {

        /// <summary>
        /// Serves as the Data that the Control markup accesses and displays
        /// </summary>
        public TaskItemModel TaskItem {
            get => this.DataContext as TaskItemModel;
        }

        public TaskItemControl() {
            this.InitializeComponent();

            //Update the Bindings whenever the DataContext is changed
            this.DataContextChanged += (s, e) => Bindings.Update();
        }
    }
}
