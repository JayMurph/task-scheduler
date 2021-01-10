using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_data_access_standard.Repositories {
    public class TaskItemRepository : ITaskItemRepository{

        private readonly DataTable taskTable;
        private readonly SQLiteDataAdapter taskAdapter;

        private readonly DataTable frequencyTable;
        private readonly SQLiteDataAdapter frequencyAdapter;
        public TaskItemRepository(string connStr) {

            taskAdapter = NewTaskItemAdapter(connStr);

            taskTable = new DataTable("Tasks");
            taskAdapter.FillSchema(taskTable, SchemaType.Source);
            taskAdapter.Fill(taskTable);

            frequencyTable = new DataTable("Frequencies");
            frequencyAdapter.FillSchema(frequencyTable, SchemaType.Source);
            frequencyAdapter.Fill(frequencyTable);

        }
        private static SQLiteDataAdapter NewNotificationFrequencyAdapter(string connStr) {
            SQLiteConnection conn = new SQLiteConnection(connStr);

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

        private static SQLiteDataAdapter NewTaskItemAdapter(string connectionStr) {
            SQLiteConnection conn = new SQLiteConnection(connectionStr);

            SQLiteDataAdapter taskAdapter = new SQLiteDataAdapter("SELECT * FROM Tasks", conn);

            taskAdapter.InsertCommand =
                new SQLiteCommand(
                    "INSERT INTO Tasks VALUES(" +
                    "@id, " +
                    "@title, " +
                    "@description, " +
                    "@startTime, " +
                    "@lastNotificationTime, " +
                    "@frequencyType," +
                    "@r," +
                    "@g," +
                    "@b)",
                    conn
                );

            taskAdapter.InsertCommand.Parameters.Add("@id", DbType.String, 1, "Id");
            taskAdapter.InsertCommand.Parameters.Add("@startTime", DbType.String, 1, "StartTime");
            taskAdapter.InsertCommand.Parameters.Add("@title", DbType.String, 1, "Title");
            taskAdapter.InsertCommand.Parameters.Add("@description", DbType.String, 1, "Description");
            taskAdapter.InsertCommand.Parameters.Add("@lastNotificationTime", DbType.String, 1, "LastNotificationTime");
            taskAdapter.InsertCommand.Parameters.Add("@frequencyType", DbType.String, 1, "FrequencyType");
            taskAdapter.InsertCommand.Parameters.Add("@r", DbType.Int64, 1, "R");
            taskAdapter.InsertCommand.Parameters.Add("@g", DbType.Int64, 1, "G");
            taskAdapter.InsertCommand.Parameters.Add("@b", DbType.Int64, 1, "B");

            taskAdapter.DeleteCommand = new SQLiteCommand("DELETE FROM Tasks WHERE Id=@id", conn);
            taskAdapter.DeleteCommand.Parameters.Add("@id", DbType.String, 1, "Id");

            taskAdapter.UpdateCommand =
                new SQLiteCommand(
                    "UPDATE Tasks SET " +
                    "StartTime=@startTime, " +
                    "Title=@title, " +
                    "Description=@description, " +
                    "LastNotificationTime=@lastNotificationTime, " +
                    "FrequencyType=@frequencyType, " +
                    "R=@r, " +
                    "G=@g, " +
                    "B=@b " +
                    "WHERE Id=@id",
                    conn
                );

            taskAdapter.UpdateCommand.Parameters.Add("@id", DbType.String, 1, "Id");
            taskAdapter.UpdateCommand.Parameters.Add("@startTime", DbType.String, 1, "StartTime");
            taskAdapter.UpdateCommand.Parameters.Add("@title", DbType.String, 1, "Title");
            taskAdapter.UpdateCommand.Parameters.Add("@description", DbType.String, 1, "Description");
            taskAdapter.UpdateCommand.Parameters.Add("@lastNotificationTime", DbType.String, 1, "LastNotificationTime");
            taskAdapter.UpdateCommand.Parameters.Add("@frequencyType", DbType.String, 1, "FrequencyType");
            taskAdapter.UpdateCommand.Parameters.Add("@r", DbType.Int64, 1, "R");
            taskAdapter.UpdateCommand.Parameters.Add("@g", DbType.Int64, 1, "G");
            taskAdapter.UpdateCommand.Parameters.Add("@b", DbType.Int64, 1, "B");

            return taskAdapter;
        }

        public bool Add(TaskItemDAL taskItemDAL) {

            //create new row 
            DataRow row = taskTable.NewRow();


            try {
                //set all fields of row
                row.SetField("Id", taskItemDAL.Id.ToString());
                row.SetField("StartTime", taskItemDAL.StartTime.ToString());
                row.SetField("Title", taskItemDAL.Title);
                row.SetField("Description", taskItemDAL.Description);
                row.SetField("LastNotificationTime", taskItemDAL.LastNotificationTime.ToString());
                row.SetField("FrequencyType", taskItemDAL.FrequencyType);
                row.SetField("R", taskItemDAL.R);
                row.SetField("G", taskItemDAL.G);
                row.SetField("B", taskItemDAL.B);
            }
            catch {
                //delete the new row since data could not be added
                row.Delete();
                return false;
            }

            taskTable.Rows.Add(row);
            return true;
        }

        public bool Delete(TaskItemDAL taskItemDAL) {
            return Delete(taskItemDAL.Id);
        }

        public bool Delete(object id) {
            //find the item to delete
            var findQuery = from row in taskTable.AsEnumerable()
                            where row.Field<string>("Id") == id.ToString()
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

        public IEnumerable<TaskItemDAL> GetAll() {

            List<TaskItemDAL> taskItems = new List<TaskItemDAL>();

            foreach(DataRow row in taskTable.AsEnumerable()) {
                taskItems.Add(DataToTaskItemDAL(row)); 
            }

            return taskItems;
        }

        public TaskItemDAL GetById(object id) {
            var findQuery = from row in taskTable.AsEnumerable()
                            where row.Field<string>("Id") == id.ToString()
                            select row;

            if(findQuery.Count() != 1) {
                return null;
            }
            else {
                return DataToTaskItemDAL(findQuery.First());
            }
        }

        public bool Update(TaskItemDAL taskItemDAL) {
            var findQuery = from row in taskTable.AsEnumerable()
                            where row.Field<string>("Id") == taskItemDAL.Id.ToString()
                            select row;

            if(findQuery.Count() != 1) {
                return false;
            }
            else {
                DataRow row = findQuery.First();

                try {
                    row.BeginEdit();
                    row.SetField("StartTime", taskItemDAL.StartTime.ToString());
                    row.SetField("Title", taskItemDAL.Title);
                    row.SetField("Description", taskItemDAL.Description);
                    row.SetField("LastNotificationTime", taskItemDAL.LastNotificationTime.ToString());
                    row.SetField("FrequencyType", taskItemDAL.FrequencyType);
                    row.SetField("R", taskItemDAL.R);
                    row.SetField("G", taskItemDAL.G);
                    row.SetField("B", taskItemDAL.B);
                    row.EndEdit();
                }
                catch {
                    row.CancelEdit();
                    return false;
                }

                return true;
            }
        }

        public void Dispose() {
            taskAdapter?.Dispose();
        }

        public void Save() {
            taskAdapter.Update(taskTable);
        }

        private static TaskItemDAL DataToTaskItemDAL(DataRow row) {

            return new TaskItemDAL() {
                Id = Guid.Parse(row.Field<string>("Id")),
                StartTime = DateTime.Parse(row.Field<string>("StartTime")),
                Title = row.Field<string>("Title"),
                Description = row.Field<string>("Description"),
                LastNotificationTime = DateTime.Parse(row.Field<string>("LastNotificationTime")),
                FrequencyType = row.Field<string>("FrequencyType"),
                R = (byte)row.Field<long>("R"),
                G = (byte)row.Field<long>("G"),
                B = (byte)row.Field<long>("B")
            };
        }

    }
}
