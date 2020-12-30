using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using task_scheduler_entities;
using task_scheduler_application;
using task_scheduler_application.Periods;

namespace task_scheduler {

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
            private AddTaskUseCaseFactory addTaskFactory;
            public AddTaskUseCase AddTask { get => addTaskFactory.New(); }
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

        class TaskRepository : IRepository<ITaskItem> {
            public bool Delete(ITaskItem t) {
                return false;
            }

            public bool Insert(ITaskItem t) {
                return false;
            }

            public IEnumerable<ITaskItem> Select(Predicate<ITaskItem> predicate) {
                return new List<ITaskItem>();
            }

            public bool Update(ITaskItem t) {
                return false;
            }

            bool IRepository<ITaskItem>.SaveChanges() {
                throw new NotImplementedException();
            }
        }

        private static void FactoryHarness() {
            var notificationManager = new BasicNotificationManager();
            var taskManager = new BasicTaskManager();
            var clock = new RealTimeClock();
            var taskRepo = new TaskRepository();

            foreach (ITaskItem task in taskRepo.Select(x => true)) {
                taskManager.Add(task);
            }

            var userController = new UserController(notificationManager, taskManager, clock, taskRepo);

            //

            var uc = userController.AddTask;

            uc.Input = new AddTaskUseCaseInput {
                Title = "Test",
                Comment = "Test",
                Colour = new Colour(1, 1, 1, 1),
                StartTime = DateTime.Now
            };
            uc.Execute();

            var output = uc.Output;
        }

        static void Main(string[] args) {
            //TaskHarness();
            //GuidHarness();
            FactoryHarness();
        }
    }
}
