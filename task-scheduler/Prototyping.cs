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
using task_scheduler_application.UseCases;
using task_scheduler_application.Frequencies;
using task_scheduler_data_access_standard.DataObjects;
using task_scheduler_data_access_standard.Repositories;
using System.Data.SQLite;
using System.Data;

namespace task_scheduler {

    class Prototyping {

        static void Main(string[] args) {

            TaskItemRepository taskRepo = new TaskItemRepository("Data Source=../../testdb.db");
            FrequencyRepository freqRepo = new FrequencyRepository("Data Source=../../testdb.db");

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
            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach(NotificationFrequencyDAL n in freqRepo.GetAll()) {
                Console.WriteLine(n.TaskId + " " + n.Time);
            }
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);

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
    }
}
