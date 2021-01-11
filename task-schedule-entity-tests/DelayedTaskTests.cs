using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using task_scheduler_entities;

namespace task_schedule_entity_tests {
    [TestClass]
    public class DelayedTaskTests {

        #region Test Fixtures
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

        #endregion

        [TestMethod]
        public void PerformsActionAfterDueTime() {
            ControlledClock clock = new ControlledClock(DateTime.Now);
            bool flag = false;

            using (DelayedTask delayedTask = new DelayedTask(
                () => { flag = true; },
                clock.Now.AddSeconds(2),
                clock
                )) {
                clock.AddSeconds(2);
                Thread.Sleep(2);        
            }

            Assert.AreEqual(true, flag);
        }

        [TestMethod]
        public void DoesNotPerformActionPriorToDueTime() {
            ControlledClock clock = new ControlledClock(DateTime.Now);
            bool flag = false;

            using (DelayedTask delayedTask = new DelayedTask(
                () => { flag = true; },
                clock.Now.AddSeconds(2),
                clock
                )) {
                clock.AddSeconds(1);
                Thread.Sleep(2);        
            }

            Assert.AreEqual(false, flag);
        }

        [TestMethod]
        public void DoesNotPerformActionWhenCancelled() {
            ControlledClock clock = new ControlledClock(DateTime.Now);
            bool flag = false;

            using (DelayedTask delayedTask = new DelayedTask(
                () => { flag = true; },
                clock.Now.AddSeconds(2),
                clock
                )) {
                delayedTask.Cancel();
                clock.AddSeconds(3);
                Thread.Sleep(2);        
            }

            Assert.AreEqual(false, flag);
        }

    }
}
