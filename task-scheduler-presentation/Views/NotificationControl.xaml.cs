using task_scheduler_presentation.Models;
using Windows.UI.Xaml.Controls;

namespace task_scheduler_presentation.Views {
    public sealed partial class NotificationControl : UserControl {

        /// <summary>
        /// Serves as the Data that the Control markup accesses and displays
        /// </summary>
        public NotificationModel Notification {
            get => this.DataContext as NotificationModel;
        }

        public NotificationControl() {
            this.InitializeComponent();

            //Update the Bindings whenever the DataContext is changed
            this.DataContextChanged += (s, e) => Bindings.Update();
        }
    }
}
