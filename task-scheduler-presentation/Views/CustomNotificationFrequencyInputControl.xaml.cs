using System;
using System.Text.RegularExpressions;
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

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// Displays textboxes for day, hour and minutes input. Validates the content of the textboxes
    /// as they are changed, to ensure that the contents are valid day, minute, or hour values
    /// </summary>
    public sealed partial class CustomNotificationFrequencyInputControl : UserControl {

        private string prevMinuteInput = "";
        private string prevHourInput = "";
        private string prevDayInput = "";

        //allowing for day values of 0 - 365
        private static Regex dayRegex = 
            new Regex(@"^([0-9]|([1-9][0-9])|([1-2][0-9][0-9])|(3[0-5][0-9])|(36[0-5]))$");

        private static Regex minuteRegex =
            new Regex(@"^([0-9]|[1-5][0-9])$");

        private static Regex hourRegex = 
            new Regex(@"^([0-9]|(1[0-9])|(2[0-3]))$");

        /// <summary>
        /// Returns the values of the Controls input fields as 
        /// </summary>
        public TimeSpan NotificationFrequencyInput {
            get {
                return new TimeSpan(
                    (dayInput.Text == "" ? 0 : int.Parse(dayInput.Text)),
                    (hourInput.Text == "" ? 0 : int.Parse(hourInput.Text)),
                    (minuteInput.Text == "" ? 0 : int.Parse(minuteInput.Text)),
                    0
                );
            }
            set {
                if(value == TimeSpan.Zero) {
                    dayInput.Text = "";
                    hourInput.Text = "";
                    minuteInput.Text = "";
                }
                else {
                    //set input fields to appropriate values from value TimeSpan
                    dayInput.Text = value.TotalDays.ToString();
                    hourInput.Text = value.TotalHours.ToString();
                    minuteInput.Text = value.TotalMinutes.ToString();
                }
            }
        }

        public CustomNotificationFrequencyInputControl() {
            this.InitializeComponent();
        }

        private void DayInput_TextChanged(object sender, TextChangedEventArgs e) {
            dayInput.Text = dayInput.Text.Trim();

            DisallowBadInput(dayInput, dayRegex, ref prevDayInput);
        }

        private void HourInput_TextChanged(object sender, TextChangedEventArgs e) {
            hourInput.Text = hourInput.Text.Trim();

            DisallowBadInput(hourInput, hourRegex, ref prevHourInput);
        }

        private void MinuteInput_TextChanged(object sender, TextChangedEventArgs e) {
            minuteInput.Text = minuteInput.Text.Trim();

            DisallowBadInput(minuteInput, minuteRegex, ref prevMinuteInput);
        }

        private void DisallowBadInput(TextBox textBox, Regex inputFormat, ref string prevGoodInput) {
            if(textBox.Text == "") {

            }
            else if (inputFormat.IsMatch(textBox.Text)){
                prevGoodInput = textBox.Text;
            }
            else {
                textBox.Text = prevGoodInput;
            }
        }
    }
}
