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

            using (var conn = new SQLiteConnection("Data Source=../../testdb.db")) {

                using (var insertCommand = 
                    new SQLiteCommand($"INSERT INTO Tasks VALUES('{Guid.NewGuid()}', 'TestTitle', 'TestDescription', '{DateTime.Now}')", conn)) {

                    conn.Open();
                    insertCommand.ExecuteNonQuery();
                    conn.Close();
                }

                using (var queryCommand = new SQLiteCommand("SELECT * FROM Tasks", conn)) {
                    conn.Open();

                    using (var reader = queryCommand.ExecuteReader()) {

                        while (reader.Read()) {

                            Guid id = Guid.Parse((string)reader["Id"]);
                            string title = (string)reader["Title"];
                            string description = (string)reader["Description"];
                            DateTime startTime = DateTime.Parse((string)reader["StartTime"]);

                            Console.WriteLine($"Id = {id}");
                            Console.WriteLine($"Title = {title}");
                            Console.WriteLine($"Description = {description}");
                            Console.WriteLine($"StartTime = {startTime}");
                        }
                    }

                    conn.Close();
                }

                using (var deleteCommand = new SQLiteCommand("DELETE FROM Tasks", conn)) {
                    conn.Open();

                    deleteCommand.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }
    }
}
