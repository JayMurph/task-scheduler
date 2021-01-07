using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access_standard {
    public static class DataAccess {
        public static void InitializeDatabase(string connectionStr) {
            try {
                //create database tables
                using (var conn = new System.Data.SQLite.SQLiteConnection(connectionStr)) {
                    //create Tasks table
                    using (var command = new System.Data.SQLite.SQLiteCommand()) {
                        command.Connection = conn;
                        command.CommandText =
                            "CREATE TABLE IF NOT EXISTS 'Tasks'" +
                                "( " +
                                "'Id'    TEXT NOT NULL UNIQUE, " +
                                "'Title' TEXT NOT NULL, " +
                                "'Description'   TEXT NOT NULL, " +
                                "'StartTime' TEXT NOT NULL, " +
                                "'LastNotificationTime'  TEXT NOT NULL, " +
                                "'FrequencyType' TEXT NOT NULL, " +
                                "'R' INTEGER NOT NULL, " +
                                "'G' INTEGER NOT NULL, " +
                                "'B' INTEGER NOT NULL, " +
                                "PRIMARY KEY('Id')" +
                                ") ";

                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                    }

                    //create Frequencies table
                    using (var command = new System.Data.SQLite.SQLiteCommand()) {
                        command.Connection = conn;
                        command.CommandText =
                            "CREATE TABLE 'Frequencies'" +
                                "( " +
                                "'TaskItemId'    TEXT NOT NULL UNIQUE, " +
                                "'Frequency' TEXT NOT NULL, " +
                                "FOREIGN KEY('TaskItemId') REFERENCES 'Tasks'('Id'), " +
                                "PRIMARY KEY('TaskItemId') " +
                                ") ";

                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch {

            }
        }
    }
}
