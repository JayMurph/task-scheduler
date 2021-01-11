using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_data_access_standard.Repositories {
    class NotificationFrequencyRepository : INotificationFrequencyRepository {

        private readonly DataTable frequencyTable;
        private readonly SQLiteDataAdapter frequencyAdapter;

        public NotificationFrequencyRepository(string connStr) {
            frequencyAdapter = NewNotificationFrequencyAdapter(connStr);

            frequencyTable = new DataTable("Frequencies");
            frequencyAdapter.FillSchema(frequencyTable, SchemaType.Source);
            frequencyAdapter.Fill(frequencyTable);
        }

        public bool Add(NotificationFrequencyDAL notificationFrequency) {
            throw new NotImplementedException();
        }

        public bool Delete(NotificationFrequencyDAL notificationFrequency) {
            throw new NotImplementedException();
        }

        public bool Delete(object id) {
            throw new NotImplementedException();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        public IEnumerable<NotificationFrequencyDAL> GetAll() {
            throw new NotImplementedException();
        }

        public NotificationFrequencyDAL GetById(object id) {
            throw new NotImplementedException();
        }

        public void Save() {
            throw new NotImplementedException();
        }

        public bool Update(NotificationFrequencyDAL notificationFrequency) {
            throw new NotImplementedException();
        }

        private static SQLiteDataAdapter NewNotificationFrequencyAdapter(string connectionStr) {
            SQLiteConnection conn = new SQLiteConnection(connectionStr);

            SQLiteDataAdapter frequencyAdapter = new SQLiteDataAdapter("SELECT * FROM Frequencies", conn);

            frequencyAdapter.InsertCommand = new SQLiteCommand(
                "INSERT INTO Frequencies VALUES(" +
                "@taskId, " +
                "@time)",
                conn
            );

            frequencyAdapter.InsertCommand.Parameters.Add("@taskId", DbType.String, 1, "TaskId");
            frequencyAdapter.InsertCommand.Parameters.Add("@time", DbType.String, 1, "Time");

            frequencyAdapter.UpdateCommand = new SQLiteCommand(
                "UPDATE Frequencies SET " +
                "Time=@time " + 
                "WHERE TaskId=@taskId",
                conn
            );

            frequencyAdapter.UpdateCommand.Parameters.Add("@time", DbType.String, 1, "Time");
            frequencyAdapter.UpdateCommand.Parameters.Add("@taskId", DbType.String, 1, "TaskId");

            frequencyAdapter.DeleteCommand = 
                new SQLiteCommand("DELETE FROM Frequencies WHERE TaskId=@taskId", conn);
            frequencyAdapter.DeleteCommand.Parameters.Add("@taskId", DbType.String, 1, "TaskId");

            return frequencyAdapter;
        }
    }
}
