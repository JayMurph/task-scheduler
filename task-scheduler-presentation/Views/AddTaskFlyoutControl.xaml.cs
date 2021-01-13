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
    public sealed partial class AddTaskFlyoutControl: UserControl{

        public IAddTaskView Owner { get; set; }

        public string Title { get => titleInput.Text; set => titleInput.Text = value; }

        public string Description { get => descriptionInput.Text; set => descriptionInput.Text = value; }

        public DateTime StartTime { 
            get => dateInput.Date.Value.LocalDateTime + timeInput.Time;
            set {
                dateInput.Date = value.Date;
                timeInput.Time = value.TimeOfDay;
            }
        }
        public Windows.UI.Color Color { get => colorInput.Color; set => colorInput.Color = value; }
        public string FrequencyType { 
            get => ((ComboBoxItem)frequencyComboBox.SelectedItem).Content.ToString();
            set { /*need to intelligently set the item*/}
        }
        public TimeSpan CustomFrequency {
            get {
                //need to ensure all custom time values are valid numbers
                return new TimeSpan(int.Parse(customDaysInput.Text), int.Parse(customHoursInput.Text), int.Parse(customMinutesInput.Text), 0, 0);
            }
            set {
                customDaysInput.Text = value.TotalDays.ToString();
                customHoursInput.Text = value.TotalHours.ToString();
                customMinutesInput.Text = value.TotalMinutes.ToString();
            }
        }
        public string Error {
            get => "";
            set {
                //do something other than this
                var dialog = new Windows.UI.Popups.MessageDialog(value);
                dialog.ShowAsync();
            }
        }


        public AddTaskFlyoutControl() {
            this.InitializeComponent();

            DataContext = this;
            
            dateInput.MinDate = DateTime.Today;
            dateInput.Date = DateTime.Today;
            timeInput.Time = DateTime.Now - DateTime.Today;

            //add frequency type combo box items
            foreach(string type in App.UserController.FrequencyTypeStrings) {
                frequencyComboBox.Items.Add(new ComboBoxItem() { Content = type });
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e) {
            App.UserController.CreateTask(Owner);
        }

        private void FrequencyComboBox_FrequencyChanged(object sender, SelectionChangedEventArgs e) {

            if(customFrequencyPanel != null && frequencyComboBox != null) {
                //need to get the string out of here.
                //maybe some method in the controller.
                if(((ComboBoxItem)frequencyComboBox.SelectedItem).Content.ToString() == "Custom") {
                    customFrequencyPanel.Visibility = Visibility.Visible;
                }
                else {
                    customFrequencyPanel.Visibility = Visibility.Collapsed;
                }
            }

        }
    }

}
