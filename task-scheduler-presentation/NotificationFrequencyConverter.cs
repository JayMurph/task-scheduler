using System;
using System.Text;
using Windows.UI.Xaml.Data;

namespace task_scheduler_presentation {

    /// <summary>
    /// Converts a TimeSpan to a formatted string, for the purposes of being displayed in
    /// <see cref="task_scheduler_presentation.Views.TaskItemControl"/>
    /// </summary>
    public class NotificationFrequencyConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is TimeSpan time) {
                if (time != TimeSpan.Zero) {
                    /*
                     * builds the string representation of the Custom Notification Frequency that
                     * will be returned
                     */
                    StringBuilder builder = new StringBuilder();
                    builder.Append("Every ");

                    //add the days if the incoming time value has days in it
                    if (time.Days > 0) {
                        string dayStr = "Day";

                        if (time.Days > 1) {
                            dayStr += "s";
                        }

                        builder.AppendFormat("{0} {1} ", time.ToString("%d"), dayStr);
                    }

                    //add hours if the incoming time value has hours in it
                    if (time.Hours > 0) {
                        string hourStr = "Hour";

                        if (time.Hours > 1) {
                            hourStr += "s";
                        }

                        builder.AppendFormat("{0} {1} ", time.ToString("%h"), hourStr);
                    }

                    //add minutes if the incoming time value has minutes in it
                    if (time.Minutes > 0) {
                        string minuteStr = "Minute";

                        if (time.Minutes > 1) {
                            minuteStr += "s";
                        }

                        builder.AppendFormat("{0} {1}", time.ToString("%m"), minuteStr);
                    }

                    return builder.ToString();
                }
                else {
                    return "";
                }
            }
            else {
                string timeStr = value.ToString();
                if (timeStr == TimeSpan.Zero.ToString()) {
                    return "";
                }
                else {
                    return timeStr;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return TimeSpan.Parse((string)value);
        }
    }
}