using System;
using System.Collections.Generic;
using System.Linq;

namespace task_scheduler_entities {
    /// <summary>
    /// Basic, threadsafe implementation of the INotificationManager interface
    /// </summary>
    public class BasicNotificationManager : INotificationManager {
        /// <summary>
        /// Invoked when a new Notification is added to the INotificationManager
        /// </summary>
        public event EventHandler<Notification> NotificationAdded;

        private List<Notification> notifications = new List<Notification>();

        /// <summary>
        /// Invokes the NotificationAdded delegates with the given Notification
        /// </summary>
        /// <param name="notification">
        /// Provided as a parameter to NoficationAdded delegates
        /// </param>
        protected void OnAdded(Notification notification) {
            NotificationAdded?.Invoke(this, notification);
        }

        /// <summary>
        /// Gives the BasicNotificationManager a new Notification to hold in
        /// its collection
        /// </summary>
        /// <param name="notification">
        /// To be added to the BasicNotificationManager's collection.
        /// </param>
        public void Add(Notification notification) {
            if(notification != null) {
                lock (notification) {
                    notifications.Add(notification);
                }
                OnAdded(notification);
            }
        }

        /// <summary>
        /// Returns a copy of the BasicNotificationManager's collection of Notifications
        /// </summary>
        /// <returns></returns>
        public List<Notification> GetAll() {
            lock (notifications) {
                return new List<Notification>(notifications);
            }
        }

        /// <summary>
        /// Removes the given Notification from the collection being managed by
        /// the BasicNotificationManager
        /// </summary>
        /// <param name="notification">
        /// To be removed from the BasicNotificationManager's collection.
        /// </param>
        /// <returns>
        /// True if the given notification was removed from the
        /// BasicNotificationManager's collection, otherwise false.
        /// </returns>
        public bool Remove(Notification notification) {
            if(notification != null) {
                lock (notification) {
                    return notifications.Remove(notification);
                }
            }
            return false;
        }

        /// <summary>
        /// Removes the Notifications that were produced by the TaskItem
        /// represented by the taskId parameter from the collection of
        /// Notifications being managed by the BasicNotificationManager
        /// </summary>
        /// <param name="taskId">
        /// Unique identifier of a TaskItem whose Notifications are to be
        /// removed from the BasicNotificationManager's collection.
        /// </param>
        /// <returns>
        /// True if all the Notifications found to have been produced by the
        /// TaskItem with the given taskId were removed from the
        /// BasicNotificationManager, otherwise false.
        /// </returns>
        public bool Remove(Guid taskId) {
            bool res = true;

            lock (notifications) {
                IEnumerable<Notification> taskNotifications = notifications.Where(x => x.Producer.ID == taskId);
                for(int i =  taskNotifications.Count() - 1; i >= 0; i--) {
                    res = res && Remove(taskNotifications.ElementAt(i));
                }
            }

            return res;
        }
    }
}
