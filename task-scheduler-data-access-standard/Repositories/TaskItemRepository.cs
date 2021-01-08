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

        private readonly DataTable table;
        private readonly SQLiteDataAdapter adapter;

        public TaskItemRepository(string connStr) {

            SQLiteConnection conn = new SQLiteConnection(connStr);

            adapter = new SQLiteDataAdapter("SELECT * FROM Tasks", conn);

            adapter.InsertCommand =
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

            adapter.InsertCommand.Parameters.Add("@id", DbType.String, 1, "Id");
            adapter.InsertCommand.Parameters.Add("@startTime", DbType.String, 1, "StartTime");
            adapter.InsertCommand.Parameters.Add("@title", DbType.String, 1, "Title");
            adapter.InsertCommand.Parameters.Add("@description", DbType.String, 1, "Description");
            adapter.InsertCommand.Parameters.Add("@lastNotificationTime", DbType.String, 1, "LastNotificationTime");
            adapter.InsertCommand.Parameters.Add("@frequencyType", DbType.String, 1, "FrequencyType");
            adapter.InsertCommand.Parameters.Add("@r", DbType.Int64, 1, "R");
            adapter.InsertCommand.Parameters.Add("@g", DbType.Int64, 1, "G");
            adapter.InsertCommand.Parameters.Add("@b", DbType.Int64, 1, "B");

            adapter.DeleteCommand = new SQLiteCommand("DELETE FROM Tasks WHERE Id=@id", conn);
            adapter.DeleteCommand.Parameters.Add("@id", DbType.String, 1, "Id");

            adapter.UpdateCommand =
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

            adapter.UpdateCommand.Parameters.Add("@id", DbType.String, 1, "Id");
            adapter.UpdateCommand.Parameters.Add("@startTime", DbType.String, 1, "StartTime");
            adapter.UpdateCommand.Parameters.Add("@title", DbType.String, 1, "Title");
            adapter.UpdateCommand.Parameters.Add("@description", DbType.String, 1, "Description");
            adapter.UpdateCommand.Parameters.Add("@lastNotificationTime", DbType.String, 1, "LastNotificationTime");
            adapter.UpdateCommand.Parameters.Add("@frequencyType", DbType.String, 1, "FrequencyType");
            adapter.UpdateCommand.Parameters.Add("@r", DbType.Int64, 1, "R");
            adapter.UpdateCommand.Parameters.Add("@g", DbType.Int64, 1, "G");
            adapter.UpdateCommand.Parameters.Add("@b", DbType.Int64, 1, "B");


            table = new DataTable("Tasks");

            adapter.FillSchema(table, SchemaType.Source);
            adapter.Fill(table);
        }

        public bool Add(TaskItemDAL taskItemDAL) {

            //create new row 
            DataRow row = table.NewRow();


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

            table.Rows.Add(row);
            return true;
        }

        public bool Delete(TaskItemDAL taskItemDAL) {
            return Delete(taskItemDAL.Id);
        }

        public bool Delete(object id) {
            //find the item to delete
            var findQuery = from row in table.AsEnumerable()
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

            foreach(DataRow row in table.AsEnumerable()) {
                taskItems.Add(DataToTaskItemDAL(row)); 
            }

            return taskItems;
        }

        public TaskItemDAL GetById(object id) {
            var findQuery = from row in table.AsEnumerable()
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
            var findQuery = from row in table.AsEnumerable()
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
            adapter?.Dispose();
        }

        public void Save() {
            adapter.Update(table);
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
