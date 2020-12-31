using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Collections.Generic;
using task_scheduler_entities;

namespace task_schedule_entity_tests {
    [TestClass]
    public class TaskItemTests {
        internal class ControlledClock : IClock {
            private DateTime time;
            public DateTime Now {
                get => time;
            }

            public ControlledClock(DateTime initialTime) {
                this.time = initialTime;
            }

            public void AddSeconds(uint seconds) {
                time = time.AddSeconds(seconds);
            }

            public void AddSeconds(double seconds) {
                time = time.AddSeconds(seconds);
            }

            public object Clone() {
                return new ControlledClock(Now);
            }
        }
        internal class ConstPeriod : INotificationFrequency {
            TimeSpan period;
            public ConstPeriod(TimeSpan period) {
                this.period = period;
            }
            public DateTime NextNotificationTime(DateTime taskStartTime, DateTime now) {
                TimeSpan periodAccum = period;

                while(taskStartTime + periodAccum <= now) {
                    periodAccum = periodAccum.Add(period);
                }

                return taskStartTime + periodAccum;
            }

            public TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now) {
                return now.Subtract(NextNotificationTime(taskStartTime, now));
            }
        }
        internal class NotificationManager : INotificationManager {
            private List<Notification> notifications = new List<Notification>();
            public event EventHandler<Notification> NotificationAdded;

            public void Add(Notification notification) {
                lock (notifications) {
                    notifications.Add(notification);
                }
                OnNotificationAdded(notification);
            }

            public List<Notification> GetAll() {
                lock (notifications) {
                    return new List<Notification>(notifications);
                }
            }

            public bool Remove(Notification notification) {
                lock (notifications) {
                    return notifications.Remove(notification);
                }
            }

            protected void OnNotificationAdded(Notification notification) {
                NotificationAdded?.Invoke(this, notification);   
            }
        }

        [TestMethod]
        public void TaskItem_ProducesNotification() {

            ControlledClock clock = new ControlledClock(DateTime.Now);
            NotificationManager manager = new NotificationManager();
            ConstPeriod constPeriod = new ConstPeriod(new TimeSpan(0,0,2));

            TaskItem testTask = new TaskItem(
                "Test",
                "Test",
                new Colour(0, 0, 0),
                clock.Now,
                manager,
                constPeriod,
                clock
            );

            clock.AddSeconds(2.1);
            Thread.Sleep(10);

            testTask.Cancel();

            Assert.AreEqual(1, manager.GetAll().Count);
        }

        [TestMethod]
        public void TaskItem_DoesNotProductNotificationWhenCancelled() {
            ControlledClock clock = new ControlledClock(DateTime.Now);
            NotificationManager manager = new NotificationManager();
            ConstPeriod constPeriod = new ConstPeriod(new TimeSpan(0,0,2));

            TaskItem testTask = new TaskItem(
                "Test",
                "Test",
                new Colour(0, 0, 0),
                clock.Now,
                manager,
                constPeriod,
                clock
            );

            testTask.Cancel();

            clock.AddSeconds(2.1);
            Thread.Sleep(10);

            Assert.AreEqual(0, manager.GetAll().Count);
        }
    }
}
