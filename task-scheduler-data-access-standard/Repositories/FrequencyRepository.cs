using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_data_access_standard.Repositories {
    public class FrequencyRepository : IFrequencyRepository {

        private readonly DataTable table;
        private readonly SQLiteDataAdapter adapter;

        public FrequencyRepository(string connStr) {
            SQLiteConnection conn = new SQLiteConnection(connStr);

            adapter = new SQLiteDataAdapter("SELECT * FROM Frequencies", conn);

            adapter.InsertCommand = new SQLiteCommand(
                "INSERT INTO Frequencies VALUES(" +
                "@taskId, " +
                "@time)",
                conn
            );

            adapter.InsertCommand.Parameters.Add("@taskId", DbType.String, 1, "TaskId");
            adapter.InsertCommand.Parameters.Add("@time", DbType.String, 1, "Time");

            adapter.UpdateCommand = new SQLiteCommand(
                "UPDATE Frequencies SET " +
                "Time=@time " + 
                "WHERE TaskId=@taskId",
                conn
            );

            adapter.UpdateCommand.Parameters.Add("@time", DbType.String, 1, "Time");
            adapter.UpdateCommand.Parameters.Add("@taskId", DbType.String, 1, "TaskId");

            adapter.DeleteCommand = 
                new SQLiteCommand("DELETE FROM Frequencies WHERE TaskId=@taskId", conn);
            adapter.DeleteCommand.Parameters.Add("@taskId", DbType.String, 1, "TaskId");

            table = new DataTable("Frequencies");

            adapter.FillSchema(table, SchemaType.Source);
            adapter.Fill(table);
        }

        public bool Add(NotificationFrequencyDAL frequencyDAL) {
            DataRow row = table.NewRow();

            try {
                row.SetField("TaskId", frequencyDAL.TaskId);
                row.SetField("Time", frequencyDAL.Time.ToString());
            }
            catch {
                row.Delete();
                return false;
            }

            table.Rows.Add(row);
            return true;
        }

        public bool Delete(NotificationFrequencyDAL frequencyDAL) {
            return Delete(frequencyDAL.TaskId);
        }

        public bool Delete(object taskId) {
            var findQuery = from row in table.AsEnumerable()
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

            foreach(DataRow row in table.AsEnumerable()) {
                frequencyItems.Add(DataToNotificationFrequencyDAL(row));
            }

            return frequencyItems;
        }

        public NotificationFrequencyDAL GetById(object taskId) {
            var findQuery = from row in table.AsEnumerable()
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
            adapter.Update(table);
        }

        public bool Update(NotificationFrequencyDAL frequencyDAL) {
            var findQuery = from row in table.AsEnumerable()
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
            adapter?.Dispose();
        }
    }
}
