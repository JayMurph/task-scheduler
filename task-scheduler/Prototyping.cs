using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using task_scheduler_entities;
using task_scheduler_application;

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

    class Prototyping {

        private static void TaskHarness() {
            INotificationManager manager = new BasicNotificationManager();

            manager.NotificationAdded += (object s, Notification n) => { Console.WriteLine($"{n.Time} : {n.Producer.Title}"); };

            DateTime fake = DateTime.Now.Add(new TimeSpan(0, 0, 0, 10));

            ITaskItem newTask =
                new TaskItem(
                    "New task",
                    "New task",
                    new Colour(255, 255, 255, 255),
                    DateTime.Now,
                    manager,
                    new ConstantPeriod(new TimeSpan(0, 0, 5)),
                    new RealTimeClock(),
                    fake.AddSeconds(80),
                    Guid.NewGuid()
                );

            string input = "";
            while (input != "x") {
                Console.WriteLine("Waiting . . .");
                input = Console.ReadLine();
                break;
            }
        }

        private static void GuidHarness() {
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Console.WriteLine($"1 --  {id1}");
            Console.WriteLine($"2 --  {id2}");
            Console.WriteLine($"id1 == id2 ?  =  {id1 == id2}");
        }
        
        class AddTaskUseCaseInput {
            public string Title;
            public string Comment;
            public Colour Colour;
            public DateTime StartTime;
        }

        class AddTaskUseCaseOutput {
            public bool Success;
            public string Error;
        }
        class AddTaskUseCase : IUseCase<AddTaskUseCaseInput, AddTaskUseCaseOutput> {
            INotificationManager notificationManager;
            ITaskManager taskManager;
            IClock clock;
            IRepository<ITaskItem> taskRepo;

            public AddTaskUseCaseInput Input { set; get; }

            public AddTaskUseCaseOutput Output { get; private set; }

            public void Execute() {
                AddTaskUseCaseInput input = Input;

                TaskItem newItem = new TaskItem(
                    input.Title,
                    input.Comment,
                    input.Colour,
                    input.StartTime,
                    notificationManager,
                    new ConstantPeriod(new TimeSpan(0, 0, 15)),
                    clock
                    );

                taskManager.Add(newItem);
                taskRepo.Insert(newItem);

                Output = new AddTaskUseCaseOutput { Success = true };
            }

            public AddTaskUseCase(
                INotificationManager notificationManager,
                ITaskManager taskManager,
                IClock clock,
                IRepository<ITaskItem> taskRepo) {

                this.notificationManager = notificationManager;
                this.taskManager = taskManager;
                this.clock = clock;
                this.taskRepo = taskRepo;
            }
        }

        class AddTaskUseCaseFactory : IUseCaseFactory<AddTaskUseCase> {

            INotificationManager notificationManager;
            ITaskManager taskManager;
            IClock clock;
            IRepository<ITaskItem> taskRepo;

            public AddTaskUseCaseFactory(
                INotificationManager notificationManager,
                ITaskManager taskManager,
                IClock clock,
                IRepository<ITaskItem> taskRepo) {

                this.notificationManager = notificationManager;
                this.taskManager = taskManager;
                this.clock = clock;
                this.taskRepo = taskRepo;
            }

            public AddTaskUseCase New() {
                return new AddTaskUseCase(
                    notificationManager,
                    taskManager,
                    clock,
                    taskRepo
                );
            }
        }

        class UserController {
            AddTaskUseCaseFactory addTaskFactory;
            public UserController(
                INotificationManager notificationManager,
                ITaskManager taskManager,
                IClock clock,
                IRepository<ITaskItem> taskRepo) {

                addTaskFactory = new AddTaskUseCaseFactory(
                    notificationManager,
                    taskManager,
                    clock,
                    taskRepo
                    );
            }
        }

        private static void FactoryHarness() {

        }

        static void Main(string[] args) {
            //TaskHarness();
            //GuidHarness();
            FactoryHarness();
        }
    }
}
