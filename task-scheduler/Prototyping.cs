using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using task_scheduler_entities;
using task_scheduler_application;
using task_scheduler_application.DTO;
using task_scheduler_application.UseCases;
using task_scheduler_application.NotificationFrequencies;
using task_scheduler_data_access_standard.DataObjects;
using task_scheduler_data_access_standard.Repositories;
using System.Data;

namespace task_scheduler {

    class Prototyping {

        static volatile bool stopSignal = false;

        static void Main(string[] args) {
            //TaskItemProfiling();

            //SpinWaitTesting();

            TaskItemDTO taskA = new TaskItemDTO();
            TaskItemDTO taskB = taskA;

            taskA.Title = "1";
            taskB.Title = "2";

            Console.WriteLine($"taskA : {taskA.Title}");
            Console.WriteLine($"taskB : {taskB.Title}");



            //TaskItemRepository taskRepo = new TaskItemRepository("Data Source=../../testdb.db");
            //NotificationFrequencyRepository freqRepo = new NotificationFrequencyRepository("Data Source=../../testdb.db");

            //TaskItemDAL newTaskDAL =
            //    new TaskItemDAL(
            //        Guid.NewGuid(),
            //        "Test",
            //        "Test",
            //        DateTime.Now,
            //        DateTime.MinValue,
            //        128,
            //        128,
            //        128,
            //        "Custom"
            //    );
            //GETTING Frequencies
            //Stopwatch timer = new Stopwatch();
            //timer.Start();
            //foreach(NotificationFrequencyDAL n in freqRepo.GetAll()) {
            //    Console.WriteLine(n.TaskId + " " + n.Time);
            //}
            //timer.Stop();
            //Console.WriteLine(timer.ElapsedMilliseconds);

            //ADDING taskItem
            //taskRepo.Add(newTaskDAL);
            //taskRepo.Save();

            //ADDING Frequency
            //freqRepo.Add(new NotificationFrequencyDAL(newTaskDAL.Id, new TimeSpan(1, 0, 0)));
            //freqRepo.Save();

            //UPDATING taskItem
            //var tasks = repo.GetAll();
            //foreach(TaskItemDAL task in tasks) {
            //    task.FrequencyType = "Not-Custom";
            //    repo.Update(task);
            //}
            //repo.Save();

            //DELETING taskItem
            //var tasks = repo.GetAll();
            //foreach(TaskItemDAL task in tasks) {
            //    repo.Delete(task);
            //}
            //repo.Save();
        }

        private static void SpinWaitTesting() {
            Task task = Task.Factory.StartNew(() => {
                SpinWait.SpinUntil(() => { return stopSignal; });
                Console.WriteLine("Done");
            });

            Thread.Sleep(1000);
            Console.WriteLine("Stopping");
            stopSignal = true;
            Thread.Sleep(1000);
        }

        private static void TaskItemProfiling() {
            INotificationManager notificationManager = new BasicNotificationManager();
            ITaskManager taskManager = new BasicTaskManager();
            IClock clock = new RealTimeClock();

            List<TaskItem> tasks = new List<TaskItem>();
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < 50; i++) {
                tasks.Add(
                    new TaskItem("test", "test", new Colour(255, 255, 255),
                    DateTime.Now, notificationManager, new DailyNotificationFrequency(), clock)
                );
            }
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);

            Console.WriteLine("Hello");
            Console.ReadLine();
            timer.Restart();
            foreach (TaskItem t in tasks) {
                t.Cancel();
            }
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);
        }
    }
}
