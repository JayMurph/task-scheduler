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
using task_scheduler_data_access;
using System.Data.SQLite;
using System.Data;

namespace task_scheduler {

    class Prototyping {

        static void Main(string[] args) {

            using (var conn = new SQLiteConnection("Data Source=../../testdb.db")) {

                Guid id = Guid.NewGuid();

                using (var insertCommand = 
                    new SQLiteCommand($"INSERT INTO Tasks VALUES('{id}', 'TestTitle', 'TestDescription', '{DateTime.Now}')", conn)) {

                    conn.Open();
                    insertCommand.ExecuteNonQuery();
                    conn.Close();
                }

                using (var insertCommand = 
                    new SQLiteCommand($"INSERT INTO Frequencies VALUES('{id}', '{TimeSpan.Zero}')", conn)) {

                    conn.Open();
                    insertCommand.ExecuteNonQuery();
                    conn.Close();
                }

                DataSet set = new DataSet();
                SQLiteDataAdapter taskAdapter = new SQLiteDataAdapter("SELECT * FROM Tasks", conn);
                SQLiteDataAdapter frequencyAdapter = new SQLiteDataAdapter("SELECT * FROM Frequencies", conn);

                taskAdapter.FillSchema(set, SchemaType.Source);
                taskAdapter.Fill(set, "Tasks");

                frequencyAdapter.FillSchema(set, SchemaType.Source);
                frequencyAdapter.Fill(set, "Frequencies");

                var q = from x in set.Tables["Tasks"].AsEnumerable()
                        select x;

                foreach(DataRow d in q.AsEnumerable()) {
                    Console.WriteLine(d.Field<string>("Id"));
                }

                q = from x in set.Tables["Frequencies"].AsEnumerable()
                    where x.Field<string>("TaskId") == id.ToString()
                    select x;

                foreach(DataRow d in q.AsEnumerable()) {
                    Console.WriteLine(d.Field<string>("TaskId"));
                    Console.WriteLine(d.Field<string>("Time"));
                }

            }
        }
    }
}
