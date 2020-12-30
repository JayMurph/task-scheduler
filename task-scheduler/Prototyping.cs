using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using task_scheduler_entities;

namespace task_scheduler {
    public class RealTimeClock : IClock {
        public object Clone() {
            return new RealTimeClock();
        }

        public DateTime Now {
            get { return DateTime.Now; }
        }
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

    class TaskSchedulerApplication {
        INotificationManager notificationManager;
        ExtendedTaskManager taskManager;
    }

    class UserController {
        private readonly INotificationManager notificationManager;
        private readonly ExtendedTaskManager taskManager;
        public AddTaskUseCaseFactory AddTaskUseCaseFactory { get; private set; }

        public UserController(
            INotificationManager notificationManager,
            ExtendedTaskManager taskManager) {
            this.notificationManager = notificationManager;
            this.taskManager = taskManager;

            AddTaskUseCaseFactory = new AddTaskUseCaseFactory(taskManager);
        }
    }

    class TaskItemViewModel {
        public string Title;
        public string Comment;
        public Colour Colour;
        public DateTime StartTime;
        public string Period;
        public string Error;
    }

    class TaskItemInputModel {
        public string Title;
        public string Comment;
        public Colour Colour;
        public DateTime StartTime;
        public string Period;
    }

    interface IUseCase<in T, out U> {
        T Input { set; }
        U Output { get; }
        void Execute();
    }

    interface IUseCaseFactory<T> {
        T New();
    } 

    class ExtendedTaskManager : BasicTaskManager {
        private readonly INotificationManager notificationManager;

        public ExtendedTaskManager(INotificationManager notificationManager) {
            this.notificationManager = notificationManager;
        }
        public bool Add(
            string title,
            string comment,
            Colour colour,
            DateTime startTime,
            INotificationPeriod period,
            IClock clock
            ) {

            ITaskItem newTask =
                new TaskItem(
                    title,
                    comment,
                    colour,
                    startTime,
                    notificationManager,
                    period,
                    clock
                );

            if (!base.Add(newTask)) {
                newTask.Dispose();
                return false;
            }

            return true;
        }
    }

    class AddTaskUseCaseFactory : IUseCaseFactory<AddTaskUseCase>{
        private readonly ExtendedTaskManager taskManager;

        public AddTaskUseCaseFactory(ExtendedTaskManager taskManager) {
            this.taskManager = taskManager;
        }
        public AddTaskUseCase New() {
            return new AddTaskUseCase(taskManager);
        }
    }

    class AddTaskUseCase : IUseCase<TaskItemInputModel, TaskItemViewModel> {
        private readonly ExtendedTaskManager taskManager;

        public TaskItemInputModel Input { private get; set; }

        public TaskItemViewModel Output { get; private set; } = new TaskItemViewModel();

        public void Execute() {
            //if(taskManager.Add(
            //    Input.Title,
            //    Input.Comment,
            //    Input.Colour,
            //    Input.StartTime,
            //    )) {
            //    Output = new TaskItemViewModel {
            //        Colour = Input.Colour,
            //        Title = Input.Title,
            //        Comment = Input.Comment,
            //        StartTime = Input.StartTime
            //    };
            //}
            //else {
            //    Output = new TaskItemViewModel {
            //        Error = "Duplicate task item was provided."
            //    };
            //}
        }

        public AddTaskUseCase(ExtendedTaskManager taskManager) {
            this.taskManager = taskManager;
        }
    }

    class Prototyping {
        static void Main(string[] args) {

            INotificationManager manager = new BasicNotificationManager();
            ExtendedTaskManager taskManager = new ExtendedTaskManager(manager);

            UserController userController = new UserController(manager, taskManager);

            manager.NotificationAdded += (object s, Notification n) => { Console.WriteLine($"{n.Time} : {n.Producer.Title}"); };
            DateTime fake = DateTime.Now.Add(new TimeSpan(0, 0, 0, 10));

            ITaskItem newTask =
                new TaskItem(
                    "New task",
                    "New task",
                    new Colour(255,255,255,255),
                    DateTime.Now,
                    manager,
                    new ConstantPeriod(new TimeSpan(0, 0, 5)), 
                    new RealTimeClock(),
                    fake.AddSeconds(80),
                    Guid.NewGuid()
                );

            taskManager.Add(newTask);

            string input = "";
            while(input != "x") {
                Console.WriteLine("Waiting . . .");
                input = Console.ReadLine();
                taskManager.Remove(newTask);
                break;
            }
        }
    }
}
