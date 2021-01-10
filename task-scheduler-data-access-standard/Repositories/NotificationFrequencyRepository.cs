using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_data_access_standard.Repositories {
    public class NotificationFrequencyRepository : INotificationFrequencyRepository {

        private readonly DataTable frequencyTable;
        private readonly SQLiteDataAdapter frequencyAdapter;

        public NotificationFrequencyRepository(string connStr) {
            SQLiteConnection conn = new SQLiteConnection(connStr);

            frequencyAdapter = new SQLiteDataAdapter("SELECT * FROM Frequencies", conn);

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

            frequencyTable = new DataTable("Frequencies");

            frequencyAdapter.FillSchema(frequencyTable, SchemaType.Source);
            frequencyAdapter.Fill(frequencyTable);
        }

        public bool Add(NotificationFrequencyDAL frequencyDAL) {
            DataRow row = frequencyTable.NewRow();

            try {
                row.SetField("TaskId", frequencyDAL.TaskId);
                row.SetField("Time", frequencyDAL.Time.ToString());
            }
            catch {
                row.Delete();
                return false;
            }

            frequencyTable.Rows.Add(row);
            return true;
        }

        public bool Delete(NotificationFrequencyDAL frequencyDAL) {
            return Delete(frequencyDAL.TaskId);
        }

        public bool Delete(object taskId) {
            var findQuery = from row in frequencyTable.AsEnumerable()
                            where row.Field<string>("TaskId") == taskId.ToString()
                            select row;

            //ensure we only found 1 item
            if(findQuery.Count() == 1) {
                findQuery.First().Delete();
            }
            else {
                return false;
            }

            return true;
        }

        public IEnumerable<NotificationFrequencyDAL> GetAll() {
            List<NotificationFrequencyDAL> frequencyItems =
                new List<NotificationFrequencyDAL>();

            foreach(DataRow row in frequencyTable.AsEnumerable()) {
                frequencyItems.Add(DataToNotificationFrequencyDAL(row));
            }

            return frequencyItems;
        }

        public NotificationFrequencyDAL GetById(object taskId) {
            var findQuery = from row in frequencyTable.AsEnumerable()
                            where row.Field<string>("TaskId") == taskId.ToString()
                            select row;

            if(findQuery.Count() != 1) {
                return null;
            }
            else {
                return DataToNotificationFrequencyDAL(findQuery.First());
            }
        }

        public void Save() {
            frequencyAdapter.Update(frequencyTable);
        }

        public bool Update(NotificationFrequencyDAL frequencyDAL) {
            var findQuery = from row in frequencyTable.AsEnumerable()
                            where row.Field<string>("TaskId") == frequencyDAL.TaskId.ToString()
                            select row;

            if(findQuery.Count() != 1) {
                return false;
            }
            else {
                DataRow row = findQuery.First();

                try {
                    row.BeginEdit();
                    row.SetField("Time", frequencyDAL.Time);
                    row.EndEdit();
                }
                catch {
                    row.CancelEdit();
                    return false;
                }

                return true;
            }
        }

        private static NotificationFrequencyDAL DataToNotificationFrequencyDAL(DataRow row) {
            return new NotificationFrequencyDAL() {
                TaskId = Guid.Parse(row.Field<string>("TaskId")),
                Time = TimeSpan.Parse(row.Field<string>("Time"))
            };
        }

        public void Dispose() {
            frequencyAdapter?.Dispose();
        }
    }
}
