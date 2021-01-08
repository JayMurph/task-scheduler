using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace task_scheduler_presentation {
    public class NotificationFrequencyConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if(value is TimeSpan) {
                TimeSpan time = (TimeSpan)value;

                if(time != TimeSpan.Zero) {
                    return time.ToString("c");
                }
                else {
                    return "";
                }
            }
            else {
                string timeStr = (string)value;
                if(timeStr == TimeSpan.Zero.ToString()) {
                    return "";
                }
                else {
                    return value.ToString();
                }
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return TimeSpan.Parse((string)value);
        }
    }
}
