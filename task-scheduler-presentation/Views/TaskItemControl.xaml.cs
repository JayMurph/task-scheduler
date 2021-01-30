using System;
using System.ComponentModel;
using task_scheduler_presentation.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace task_scheduler_presentation.Views {
    public sealed partial class TaskItemControl : UserControl {

        [Browsable(true)] [Category("Action")]
        [Description("Invoked when delete context menu option is selected")]
        public event EventHandler<TaskItemModel> DeleteClick;

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

        /// <summary>
        /// Invoked when the Delete option is selected from the TaskItemControl's context menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteMenuOption_Click(object sender, RoutedEventArgs e) {
            this.DeleteClick?.Invoke(sender, TaskItem);
        }
    }
}
