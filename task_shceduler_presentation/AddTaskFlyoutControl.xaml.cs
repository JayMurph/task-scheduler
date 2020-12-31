using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace task_scheduler_presentation {
    public sealed partial class AddTaskFlyoutControl: UserControl {
        public AddTaskFlyoutControl() {
            this.InitializeComponent();
            //could set min date on dateInput
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

            string title = titleInput.Text;
            string description = descriptionInput.Text;
            DateTimeOffset? date = dateInput.Date;
            TimeSpan time = timeInput.Time;

            var uc = App.UserController.AddTaskUseCaseFactory.New();
        }
    }
}
