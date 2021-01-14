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

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// Control allowing User to input the necessary information for creating a new TaskItem
    /// </summary>
    public sealed partial class AddTaskFlyoutControl: UserControl{

        /// <summary>
        /// Reference to the page that opened the AddTaskFlyoutControl
        /// </summary>
        public IAddTaskView Owner { get; set; }

        /// <summary>
        /// Title input field for a new TaskItem
        /// </summary>
        public string Title { get => titleInput.Text; set => titleInput.Text = value; }

        /// <summary>
        /// Description input field for a new TaskItem
        /// </summary>
        public string Description { get => descriptionInput.Text; set => descriptionInput.Text = value; }

        /// <summary>
        /// Start time input field for a new TaskItem
        /// </summary>
        public DateTime StartTime { 
            get => dateInput.Date.Value.LocalDateTime + timeInput.Time;
            set {
                dateInput.Date = value.Date;
                timeInput.Time = value.TimeOfDay;
            }
        }

        /// <summary>
        /// Color input field for a new TaskItem
        /// </summary>
        public Windows.UI.Color Color { get => colorInput.Color; set => colorInput.Color = value; }

        /// <summary>
        /// Notification frequency description input field for a new TaskItem
        /// </summary>
        public string FrequencyType { 
            get => ((ComboBoxItem)frequencyComboBox.SelectedItem).Content.ToString();
            set { /*TODO: need to intelligently set the item*/}
        }

        /// <summary>
        /// Custom notification frequency input field for a new TaskItem
        /// </summary>
        public TimeSpan CustomFrequency {
            get {
                return customFrequencyInput.NotificationFrequencyInput;
            }
            set {
                customFrequencyInput.NotificationFrequencyInput = value;
            }
        }

        /// <summary>
        /// Field for transmitting errors that occured during the creation of a new TaskItem.
        /// Currently opens a pop-up to display the error info.
        /// </summary>
        public string Error {
            get => "";
            set {
                //TODO: do something other than this
                var dialog = new Windows.UI.Popups.MessageDialog(value);
                dialog.ShowAsync();
            }
        }


        /// <summary>
        /// Creates a new AddTaskFlyout and sets its input fields to default values
        /// </summary>
        public AddTaskFlyoutControl() {
            this.InitializeComponent();

            DataContext = this;

            customFrequencyInput.Visibility = Visibility.Collapsed; 
            
            dateInput.MinDate = DateTime.Today;
            dateInput.Date = DateTime.Today;
            timeInput.Time = DateTime.Now - DateTime.Today;

            //add frequency type combo box items
            foreach(string type in App.UserController.FrequencyTypeStrings) {
                frequencyComboBox.Items.Add(new ComboBoxItem() { Content = type });
            }
        }

        /// <summary>
        /// Executed when the "Create" button of the AddTaskFlyoutControl is clicked. Initiates the CreateTask Use-Case
        /// </summary>
        /// <param name="sender">
        /// Initiator of the event. unused.
        /// </param>
        /// <param name="e">
        /// Arguments for the event. unused.
        /// </param>
        private void CreateButton_Click(object sender, RoutedEventArgs e) {
            App.UserController.CreateTask(Owner);
        }

        /// <summary>
        /// Executes when a new NotificationFrequency selection is made in the Control's
        /// NotificationFrequency combo box. Opens or collapses the area for inputting a Custom
        /// Notification Frequency, depending on the item selected in the NotificationFrequency
        /// combo box
        /// </summary>
        /// <param name="sender">
        /// Initiator of the event. unused.
        /// </param>
        /// <param name="e">
        /// Arguments for the event. unused.
        /// </param>
        private void FrequencyComboBox_FrequencyChanged(object sender, SelectionChangedEventArgs e) {

            if(customFrequencyInput != null && frequencyComboBox != null) {
                /*
                 * Open the customFrequencyPanel if the "Custom" item is selected in the
                 * frequencyComboBox, otherwise close the customFrequencyPanel
                 */
                if(((ComboBoxItem)frequencyComboBox.SelectedItem).Content.ToString() == "Custom") {
                    customFrequencyInput.Visibility = Visibility.Visible;
                }
                else {
                    customFrequencyInput.Visibility = Visibility.Collapsed;
                    CustomFrequency = TimeSpan.Zero;
                }
            }
        }
    }

}
