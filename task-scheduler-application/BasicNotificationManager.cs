using System;
using System.Collections.Generic;
using System.Linq;

namespace task_scheduler_entities {
    /// <summary>
    /// Basic, threadsafe implementation of the INotificationManager interface
    /// </summary>
    public class BasicNotificationManager : INotificationManager {
        private List<Notification> notifications = new List<Notification>();
        public event EventHandler<Notification> NotificationAdded;

        protected void OnAdded(Notification notification) {
            NotificationAdded?.Invoke(this, notification);
        }

        public void Add(Notification notification) {
            lock (notification) {
                notifications.Add(notification);
            }
            OnAdded(notification);
        }

        public List<Notification> GetAll() {
            lock (notifications) {
                return new List<Notification>(notifications);
            }
        }

        public bool Remove(Notification notification) {
            lock (notification) {
                return notifications.Remove(notification);
            }
        }

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
