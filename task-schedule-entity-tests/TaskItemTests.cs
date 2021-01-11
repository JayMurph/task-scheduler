using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Collections.Generic;
using task_scheduler_entities;

namespace task_schedule_entity_tests {
    [TestClass]
    public class TaskItemTests {

        #region Supporting Interfaces
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

        internal class ConstNotificationFrequency : INotificationFrequency {
            TimeSpan period;
            public ConstNotificationFrequency(TimeSpan period) {
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

        internal class ControlledTaskItemMockDependencies {
            public ControlledClock Clock { get; set; }
            public NotificationManager Manager { get; set; }
            public ConstNotificationFrequency Frequency { get; set; }

            public ControlledTaskItemMockDependencies(TimeSpan notificationFrequency) {
                this.Clock = new ControlledClock(DateTime.Now);
                this.Manager = new NotificationManager();
                this.Frequency = new ConstNotificationFrequency(notificationFrequency);
            }
        }

        #endregion

        //A small Thread.Sleep is performed during each test to allow the actual notification 
        //timing logic running on a separate thread to register that the current time has 
        //changed and react to it, before checking the result of the operation

        [TestMethod]
        public void ProducesNotification() {
            ControlledTaskItemMockDependencies deps = new ControlledTaskItemMockDependencies(new TimeSpan(0, 0, 2));

            using (TaskItem testTask = new TaskItem(
                "Test",
                "Test",
                new Colour(0, 0, 0),
                deps.Clock.Now,
                deps.Manager,
                deps.Frequency,
                deps.Clock
            )) {
                deps.Clock.AddSeconds(2);
                Thread.Sleep(2);
            }

            Assert.AreEqual(1, deps.Manager.GetAll().Count);
        }

        [TestMethod]
        public void ProducesMultipleSequentialNotifications() {
            ControlledTaskItemMockDependencies deps = new ControlledTaskItemMockDependencies(new TimeSpan(0, 0, 2));

            using (TaskItem testTask = new TaskItem(
                "Test",
                "Test",
                new Colour(0, 0, 0),
                deps.Clock.Now,
                deps.Manager,
                deps.Frequency,
                deps.Clock
            )) {
                deps.Clock.AddSeconds(2);
                Thread.Sleep(2);
                deps.Clock.AddSeconds(2);
                Thread.Sleep(2);
            }

            Assert.AreEqual(2, deps.Manager.GetAll().Count);
        }

        [TestMethod]
        public void DoesNotProductNotificationWhenCancelled() {
            ControlledTaskItemMockDependencies deps = new ControlledTaskItemMockDependencies(new TimeSpan(0, 0, 2));

            using (TaskItem testTask = new TaskItem(
                "Test",
                "Test",
                new Colour(0, 0, 0),
                deps.Clock.Now,
                deps.Manager,
                deps.Frequency,
                deps.Clock
            )) {
                testTask.Cancel();

                deps.Clock.AddSeconds(2);
                Thread.Sleep(2);
            }

            Assert.AreEqual(0, deps.Manager.GetAll().Count);
        }

        [TestMethod]
        public void ProducesNotificationAfterFrequencyChanged() {
            ControlledTaskItemMockDependencies deps = new ControlledTaskItemMockDependencies(new TimeSpan(0, 0, 4));

            using (TaskItem testTask = new TaskItem(
                "Test",
                "Test",
                new Colour(0, 0, 0),
                deps.Clock.Now,
                deps.Manager,
                deps.Frequency,
                deps.Clock
            )) {
                ConstNotificationFrequency newFrequency = new ConstNotificationFrequency(new TimeSpan(0, 0, 2));

                testTask.ChangeFrequency(newFrequency);

                deps.Clock.AddSeconds(2);
                Thread.Sleep(2);
            }

            Assert.AreEqual(1, deps.Manager.GetAll().Count);
        }

        [TestMethod]
        public void ProducesNotificationAfterFrequencyChangeWithCorrectTime() {
            ControlledTaskItemMockDependencies deps = new ControlledTaskItemMockDependencies(new TimeSpan(0, 0, 4));

            using (TaskItem testTask = new TaskItem(
                "Test",
                "Test",
                new Colour(0, 0, 0),
                deps.Clock.Now,
                deps.Manager,
                deps.Frequency,
                deps.Clock
            )) {
                ConstNotificationFrequency newFrequency = new ConstNotificationFrequency(new TimeSpan(0, 0, 2));

                testTask.ChangeFrequency(newFrequency);

                deps.Clock.AddSeconds(2);
                Thread.Sleep(2);
            }

            Assert.AreEqual(1, deps.Manager.GetAll().Count);

            Assert.AreEqual(deps.Clock.Now, deps.Manager.GetAll()[0].Time);
        }

        [TestMethod]
        public void ProducesNotificationWithCorrectTime() {
            ControlledTaskItemMockDependencies deps = new ControlledTaskItemMockDependencies(new TimeSpan(0, 0, 2));

            using (TaskItem testTask = new TaskItem(
                "Test",
                "Test",
                new Colour(0, 0, 0),
                deps.Clock.Now,
                deps.Manager,
                deps.Frequency,
                deps.Clock
            )) {
                deps.Clock.AddSeconds(2);
                Thread.Sleep(2);
            }

            Assert.AreEqual(1, deps.Manager.GetAll().Count);

            Assert.AreEqual(deps.Clock.Now, deps.Manager.GetAll()[0].Time);
        }
    }
}
