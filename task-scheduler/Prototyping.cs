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

            TaskItemRepository repo = new TaskItemRepository("Data Source=../../testdb.db");

            //ADDING
            //repo.Add(new TaskItemDAL(Guid.NewGuid(), "Test", "Test", DateTime.Now, DateTime.MinValue, 128, 128, 128, "Custom"));
            //repo.Save();

            //UPDATING
            //var tasks = repo.GetAll();
            //foreach(TaskItemDAL task in tasks) {
            //    task.FrequencyType = "Not-Custom";
            //    repo.Update(task);
            //}
            //repo.Save();

            //DELETING
            //var tasks = repo.GetAll();
            //foreach(TaskItemDAL task in tasks) {
            //    repo.Delete(task);
            //}
            //repo.Save();
        }
    }
}
