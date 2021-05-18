using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler_application;
using task_scheduler_application.DTO;
using task_scheduler_application.UseCases.ViewNotifications;
using task_scheduler_presentation.Models;
using task_scheduler_presentation.Views;
using Windows.UI.Xaml.Media;

namespace task_scheduler_presentation.Controllers {
    public class NotificationsController {
        /// <summary>
        /// Invoked when a new Notification is created within the application
        /// </summary>
        public event EventHandler<NotificationModel> NotificationCreated;

        public static TaskSchedulerApplication TaskSchedulerApp { get => taskSchedulerApp; set => taskSchedulerApp = value; }

        public NotificationsController(INotificationsView view) {
            _view = view;
            _view.Loaded += ViewNotifications;
            TaskSchedulerApp.NotificationAdded += ReceiveNotification;
        }

        /// <summary>
        /// Invokes the NotificationCreated event subscribers.
        /// </summary>
        /// <param name="notification">
        /// a newly created Notification
        /// </param>
        protected void OnNotificationCreated(NotificationModel notification) {
            NotificationCreated?.Invoke(this, notification);
        }

        private readonly INotificationsView _view = null;

        private static task_scheduler_application.TaskSchedulerApplication taskSchedulerApp = null;

        private void ViewNotifications(object obj, EventArgs args) {
            _view.Loaded -= ViewNotifications;

            //call use-case factory to create use-case object
            var uc = TaskSchedulerApp.NewViewNotificationsUseCase();

            //execute the use-case
            ViewNotificationsOutput output = uc.Execute(new ViewNotificationsInput());

            if (output.Success) {

                List<NotificationModel> models = new List<NotificationModel>();

                //get and convert all Use-Cases Notifications output to NotificationModels
                foreach(NotificationDTO notification in output.Notifications) {

                    //convert the NotificationDTOs rgb color to a Windows brush
                    SolidColorBrush colorBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, notification.R, notification.G, notification.B));

                    models.Add(
                        new NotificationModel() {
                            Title=notification.Title,
                            Time = notification.Time,
                            Color = colorBrush
                        }
                    );

                }

                /*
                 * sort the notifications by their Time values, convert it to a list, then assign
                 * them to the view's collection to display
                 */
                _view.Notifications = 
                    new ObservableCollection<NotificationModel>(models.OrderBy(x => x.Time).ToList());

                //subscribe view to NotificationCreated event
                NotificationCreated += _view.NotificationCreatedCallback;

                //use the view's Closing event to unsubscribe it from NotificationCreated
                _view.Closing += (s, e) => {
                    NotificationCreated -= _view.NotificationCreatedCallback;
                    TaskSchedulerApp.NotificationAdded -= ReceiveNotification;
                }; 
            }
            else {
                //TODO: handle possible failure of the use-case
            }
        }

        /// <summary>
        /// kludge method. To be used for receiving Notifications when they are created in separate
        /// layers (which they always are)
        /// </summary>
        /// <param name="notification">
        /// a newly created Notification
        /// </param>
        //TODO: I don't like this method. But, since Notifications are generated in the entity layer
        //then passed to a notificationManager, we need to expose a method so that we can 'hook'
        //into that event and receive the info.
        private async void ReceiveNotification(object s, NotificationDTO notification) {

            /*
             * because creating a NotificationModel involves creating a UIElement, and this method
             * will NOT be called by the UI thread ( but instead by a TaskItem's thread) we have to
             * move the creation of the model to a UI thread
             */
            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => {

                    NotificationModel model = new NotificationModel() {
                        Title = notification.Title,
                        Time = notification.Time,
                        Color = new Windows.UI.Xaml.Media.SolidColorBrush(
                            Windows.UI.Color.FromArgb(255, notification.R, notification.G, notification.B))
                    };

                    OnNotificationCreated(model);
                }
            );
        }
    }
}
