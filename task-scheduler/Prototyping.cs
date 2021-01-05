using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using task_scheduler_entities;
using task_scheduler_application;
using task_scheduler_application.UseCases;
using task_scheduler_application.Frequencies;
using task_scheduler_data_access;
using System.Data.SQLite;

namespace task_scheduler {

    class Prototyping {

        static void Main(string[] args) {
            using (var conn = new SQLiteConnection("Data Source=testdb.db")) {
                using (var command = new SQLiteCommand("INSERT INTO Tasks VALUES('blah','blah','blah','blah')", conn)) {
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
