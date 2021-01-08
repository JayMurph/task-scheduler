using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using task_scheduler_presentation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace task_scheduler_presentation {
    public class NotificationFrequencyConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if(value is TimeSpan) {
                TimeSpan time = (TimeSpan)value;

                if(time != TimeSpan.Zero) {
                    Debug.WriteLine("1");
                    return time.ToString("c");
                }
                else {
                    Debug.WriteLine("2");
                    return "";
                }
            }
            else {
                string timeStr = (string)value;
                if(timeStr == TimeSpan.Zero.ToString()) {
                    Debug.WriteLine("3");
                    return "";
                }
                else {
                    Debug.WriteLine("4");
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

namespace task_scheduler_presentation.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskPage : Page, ITasksView{
        public ObservableCollection<TaskItemModel> TaskItems { get; set; } = new ObservableCollection<TaskItemModel>();

        //list of observable task models
        public TaskPage() {
            this.InitializeComponent();

            App.UserController.ViewTasks(this);

            //should this be done by the controller?????????????
            this.taskListView.ItemsSource = TaskItems;
        }
    }
}
