using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
    public sealed partial class AddTaskFlyoutControl : UserControl{

        /// <summary>
        /// Reference to the page that opened the AddTaskFlyoutControl
        /// </summary>
        public IAddTaskView Owner { get; set; }

        /// <summary>
        /// Title input field for a new TaskItem
        /// </summary>
        public string Title { 
            get => titleInput.Text; 
            private set => titleInput.Text = value; 
        }

        /// <summary>
        /// Description input field for a new TaskItem
        /// </summary>
        public string Description {
            get => descriptionInput.Text; 
            private set => descriptionInput.Text = value; 
        }

        /// <summary>
        /// Start time input field for a new TaskItem
        /// </summary>
        public DateTime StartTime { 
            get => dateInput.Date.Value.LocalDateTime + timeInput.Time;
            private set {
                dateInput.Date = value.Date;
                timeInput.Time = value.TimeOfDay;
            }
        }

        /// <summary>
        /// Color input field for a new TaskItem
        /// </summary>
        public Windows.UI.Color Color { 
            get => colorInput.Color; 
            private set => colorInput.Color = value; 
        }

        /// <summary>
        /// Notification frequency description input field for a new TaskItem
        /// </summary>
        public string SelectedFrequencyType { 
            get => ((ComboBoxItem)frequencyComboBox.SelectedItem).Content.ToString();
        }

        /// <summary>
        /// Custom notification frequency input field for a new TaskItem
        /// </summary>
        public TimeSpan CustomFrequency {
            get => customFrequencyInput.NotificationFrequencyInput;
            private set => customFrequencyInput.NotificationFrequencyInput = value;
            
        }

        /// <summary>
        /// Carries error messages that need to be transmitted from the application layer
        /// </summary>
        public string ApplicationErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the application encountered an error while creating a new task
        /// </summary>
        public bool ApplicationError { get; set; } = false;


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

            //TODO: have the UserController apply the frequency type strings to the combo box
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
        private async void CreateButton_Click(object sender, RoutedEventArgs e) {

            if (HasInputErrors()) {
                string errors = GetErrorOutput();
                var dialog = new Windows.UI.Popups.MessageDialog(errors);
                await dialog.ShowAsync();
            }
            else {
                App.UserController.CreateTask(Owner);

                if (ApplicationError) {
                    ApplicationError = false;
                    var dialog = new Windows.UI.Popups.MessageDialog(ApplicationErrorMessage);
                    await dialog.ShowAsync();
                    ApplicationErrorMessage = string.Empty;
                }
            }
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

        /// <summary>
        /// Returns true if there are basic errors in the input fields of the AddTaskFlyoutControl
        /// </summary>
        /// <returns>
        /// True if there are errors present in the AddTaskFlyoutControl's input fields, otherwise
        /// false
        /// </returns>
        private bool HasInputErrors() {
            return string.IsNullOrWhiteSpace(Title);
        }

        /// <summary>
        /// Returns a string describing any user-input errors in the AddTaskFlyoutControl
        /// </summary>
        /// <returns>
        /// Describes user-input errors
        /// </returns>
        private string GetErrorOutput() {
            StringBuilder errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Title)) {
                errorBuilder.AppendLine("Task title cannot be empty.");
            }

            return errorBuilder.ToString();
        }

        public void ClearFields() {
            Title = string.Empty;
            Description = string.Empty;
            StartTime = DateTime.Now;
            Color = Windows.UI.Color.FromArgb(255, 255, 255, 255);
            CustomFrequency = TimeSpan.Zero;
            frequencyComboBox.SelectedIndex = 0;
        }
    }

}
