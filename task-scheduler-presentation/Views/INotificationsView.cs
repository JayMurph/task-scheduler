using System;
using System.Collections.ObjectModel;
using task_scheduler_presentation.Models;

namespace task_scheduler_presentation.Views {

    /// <summary>
    /// Defines the necessities for a page that displays Notifications
    /// </summary>
    public interface INotificationsView {

        /// <summary>
        /// Collection of Notifications that the page will display
        /// </summary>
        ObservableCollection<NotificationModel> Notifications { get; set; }

        /// <summary>
        /// Callback to be invoked whenever a new Notification is to be added to the pages
        /// collection.
        /// </summary>
        /// <param name="source">
        /// The object that called the event.
        /// </param>
        /// <param name="newNotification">
        /// A newly generated Notification
        /// </param>
        void NotificationCreatedCallback(object source, NotificationModel newNotification);

        /// <summary>
        /// Executed when the NotificationsView page is closing
        /// </summary>
        event EventHandler Closing;
    }
}
