using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace task_scheduler {
    public interface IClock : ICloneable{
        DateTime Now { get; }
    }
    public class RealTimeClock : IClock {
        public object Clone() {
            return new RealTimeClock();
        }

        public DateTime Now {
            get { return DateTime.Now; }
        }
    }
    public interface INotificationPeriod {
        TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now);
        DateTime NextNotificationTime(DateTime taskStartTime, DateTime now);
    }
    public class ConstantPeriod : INotificationPeriod {
        private readonly TimeSpan period; 

        public ConstantPeriod(TimeSpan period) {
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

    public class DelayedTask{

        private DateTime dueTime;
        private IClock clock;
        private Task asyncTask;
        private volatile bool stopSignal = false;

        public DelayedTask(Action action, DateTime dueTime, IClock clock) {
            this.dueTime = dueTime;
            this.clock = clock;

            asyncTask = Task.Factory.StartNew(() => AsyncOperation(action));
        }

        public virtual void Cancel() {
            stopSignal = true;

            try {
                asyncTask.Wait();
            }
            catch (Exception ex){
                Console.WriteLine(ex);
            }
            finally {
                asyncTask.Dispose();
            }
        }

        protected virtual void AsyncOperation(Action action) {
            while(clock.Now < dueTime) {
                if (stopSignal) {
                    return;
                }
            }

            action();
        }

    }

    public class TaskItem {
        private INotificationPeriod period;
        private INotificationManager manager;
        private IClock clock;
        private DelayedTask notifier;
        private DateTime startTime;

        public string Title { get; set; }
        public DateTime StartTime {
            get {
                return startTime;
            }
            set {
                startTime = value;
                if (IsActive) {
                    notifier?.Cancel();
                    ScheduleNextNotification();
                }
            }
        } 
        public DateTime LastNotificationTime { get; private set; } = DateTime.MinValue;
        public bool IsActive { get; private set; } = false;


        public TaskItem(
            string title,
            DateTime startTime,
            INotificationManager manager,
            INotificationPeriod period,
            IClock clock) {

            this.startTime = startTime;
            this.LastNotificationTime = startTime;
            this.Title = title;
            this.period = period;
            this.manager = manager;
            this.clock = clock;

            while(IsOverdue()){
                PostNotification(period.NextNotificationTime(this.StartTime, this.LastNotificationTime)); 
            }

            ScheduleNextNotification();
        }

        public TaskItem(
            string title,
            DateTime startTime,
            INotificationManager manager,
            INotificationPeriod period,
            IClock clock, 
            DateTime lastNotificationTime) {

            this.startTime = startTime;
            this.LastNotificationTime = lastNotificationTime;

            this.Title = title;
            this.period = period;
            this.manager = manager;
            this.clock = clock;

            while(IsOverdue()){
                PostNotification(period.NextNotificationTime(this.StartTime, this.LastNotificationTime)); 
            }

            ScheduleNextNotification();
        }

        public bool IsOverdue() {
            return period.NextNotificationTime(this.StartTime, this.LastNotificationTime) < clock.Now;
        }

        public void Cancel() {
            notifier?.Cancel();
            IsActive = false;
        }

        public void ChangePeriod(INotificationPeriod period) {
            this.period = period;

            notifier?.Cancel();

            while(IsOverdue()){
                PostNotification(period.NextNotificationTime(this.StartTime, this.LastNotificationTime)); 
            }

            ScheduleNextNotification();
        }

        private void ScheduleNextNotification() {

            DateTime nextNotificationTime = period.NextNotificationTime(StartTime, clock.Now);

            notifier = 
                new DelayedTask(
                    () => { PostNotification(clock.Now); ScheduleNextNotification(); }, 
                    nextNotificationTime, 
                    clock
                );
        }

        private void PostNotification(DateTime timeOfNotification) {
            Notification notification = new Notification(this, timeOfNotification);

            LastNotificationTime = timeOfNotification;

            manager.Add(notification);
        }
    }
    public class Notification {
        private readonly TaskItem owner;
        private readonly DateTime timeOf;
        public TaskItem TaskItem { get => owner; }
        public DateTime TimeOf { get => timeOf; }
        public Notification(TaskItem owner, DateTime timeOf) {
            this.owner = owner;
            this.timeOf = timeOf;
        }
    }
    public interface INotificationManager {
        event EventHandler<Notification> Added;
        void Add(Notification notification);
        List<Notification> GetAll();
        bool Remove(Notification notification);
    }
    public class NotificationManager : INotificationManager {
        private List<Notification> notifications = new List<Notification>();

        public event EventHandler<Notification> Added;
        protected void OnAdded(Notification notification) {
            Added?.Invoke(this, notification);
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
    }
    class Program {
        static void Main(string[] args) {

            INotificationManager manager = new NotificationManager();
            manager.Added += (object s, Notification n) => { Console.WriteLine($"{n.TimeOf} : {n.TaskItem.Title}"); };
            DateTime fake = DateTime.Now.Add(new TimeSpan(0, 0, 0, 10));
            //TaskItem oldTask =
            //    new TaskItem(
            //        "Old task",
            //        fake,
            //        manager,
            //        period,
            //        fake.Add(new TimeSpan(0,0,0,10))
            //    );

            TaskItem newTask =
                new TaskItem(
                    "New task",
                    DateTime.Now,
                    manager,
                    new ConstantPeriod(new TimeSpan(0, 0, 5)), 
                    new RealTimeClock(),
                    fake.AddSeconds(80)
                );

            string input = "";
            while(input != "x") {
                Console.WriteLine("Waiting . . .");
                input = Console.ReadLine();
                newTask.StartTime = DateTime.Now.AddSeconds(10);
            }
        }
    }
}
