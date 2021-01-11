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

            frequencyAdapter = NewNotificationFrequencyAdapter(connStr);

            frequencyTable = new DataTable("Frequencies");
            frequencyAdapter.FillSchema(frequencyTable, SchemaType.Source);
            frequencyAdapter.Fill(frequencyTable);

        }

        #region FrequencyAdapter Init Functions
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
        #endregion

        public bool Add(TaskItemDAL taskItemDAL) {

            //create new row 
            DataRow taskRow = taskTable.NewRow();
            DataRow frequencyRow = null;

            //TODO : Get rid of magic string
            if(taskItemDAL.NotificationFrequencyType == "Custom") {

                frequencyRow = frequencyTable.NewRow();

                try {
                    frequencyRow.SetField("TaskId", taskItemDAL.Id.ToString());
                    frequencyRow.SetField("Time", taskItemDAL.CustomNotificationFrequency.ToString());
                }
                catch {
                    taskRow.Delete();
                    frequencyRow.Delete();
                    return false;
                }
            }

            try {
                //set all fields of row
                taskRow.SetField("Id", taskItemDAL.Id.ToString());
                taskRow.SetField("StartTime", taskItemDAL.StartTime.ToString());
                taskRow.SetField("Title", taskItemDAL.Title);
                taskRow.SetField("Description", taskItemDAL.Description);
                taskRow.SetField("LastNotificationTime", taskItemDAL.LastNotificationTime.ToString());
                taskRow.SetField("FrequencyType", taskItemDAL.NotificationFrequencyType);
                taskRow.SetField("R", taskItemDAL.R);
                taskRow.SetField("G", taskItemDAL.G);
                taskRow.SetField("B", taskItemDAL.B);
            }
            catch {
                //delete the new row since data could not be added
                taskRow.Delete();
                frequencyRow?.Delete();
                return false;
            }

            taskTable.Rows.Add(taskRow);

            if(frequencyRow != null) {
                frequencyTable.Rows.Add(frequencyRow);
            }

            return true;
        }

        public bool Delete(TaskItemDAL taskItemDAL) {
            return Delete(taskItemDAL.Id);
        }

        public bool Delete(object id) {

            //find the item to delete
            var findTaskQuery = from row in taskTable.AsEnumerable()
                            where row.Field<string>("Id") == id.ToString()
                            select row;

            var findFrequencyQuery = from row in frequencyTable.AsEnumerable()
                                     where row.Field<string>("TaskId") == id.ToString()
                                     select row;

            //ensure we only found 1 item
            if(findTaskQuery.Count() == 1) {

                if(findFrequencyQuery.Count() == 1) {
                    findFrequencyQuery.First().Delete();
                }

                findTaskQuery.First().Delete();
            }
            else {
                return false;
            }

            return true;
        }

        public IEnumerable<TaskItemDAL> GetAll() {

            List<TaskItemDAL> taskItems = new List<TaskItemDAL>();

            foreach(DataRow taskRow in taskTable.AsEnumerable()) {

                var findFrequencyQuery = from row in frequencyTable.AsEnumerable()
                                         where row.Field<string>("TaskId") == taskRow.Field<string>("Id")
                                         select row;

                if(findFrequencyQuery.Count() == 1) {
                    taskItems.Add(DataToTaskItemDAL(taskRow, findFrequencyQuery.First())); 
                }
                else {
                    taskItems.Add(DataToTaskItemDAL(taskRow)); 
                }
            }

            return taskItems;
        }

        public TaskItemDAL GetById(object id) {
            var findTaskQuery = from row in taskTable.AsEnumerable()
                            where row.Field<string>("Id") == id.ToString()
                            select row;

            if(findTaskQuery.Count() != 1) {
                return null;
            }
            else {

                var findFrequencyQuery = from row in frequencyTable.AsEnumerable()
                                         where row.Field<string>("TaskId") == id.ToString()
                                         select row;

                if(findFrequencyQuery.Count() == 1) {
                    return DataToTaskItemDAL(findTaskQuery.First(), findFrequencyQuery.First()); 
                }
                else {
                    return DataToTaskItemDAL(findTaskQuery.First());
                }
            }
        }

        public bool Update(TaskItemDAL taskItemDAL) {
            var findTaskQuery = from row in taskTable.AsEnumerable()
                            where row.Field<string>("Id") == taskItemDAL.Id.ToString()
                            select row;

            if(findTaskQuery.Count() != 1) {
                return false;
            }
            else {
                DataRow taskRow = findTaskQuery.First();

                var findFrequencyQuery = from row in frequencyTable.AsEnumerable()
                                         where row.Field<string>("TaskId") == taskItemDAL.Id.ToString()
                                         select row;

                try {
                    taskRow.BeginEdit();
                    taskRow.SetField("StartTime", taskItemDAL.StartTime.ToString());
                    taskRow.SetField("Title", taskItemDAL.Title);
                    taskRow.SetField("Description", taskItemDAL.Description);
                    taskRow.SetField("LastNotificationTime", taskItemDAL.LastNotificationTime.ToString());
                    taskRow.SetField("FrequencyType", taskItemDAL.NotificationFrequencyType);
                    taskRow.SetField("R", taskItemDAL.R);
                    taskRow.SetField("G", taskItemDAL.G);
                    taskRow.SetField("B", taskItemDAL.B);
                    taskRow.EndEdit();

                    if(findFrequencyQuery.Count() == 1) {
                        //if a custom frequency exists for this taskitem
                        DataRow frequencyRow = findFrequencyQuery.First();

                        if(taskItemDAL.NotificationFrequencyType != "Custom") {
                            //if taskitem no long has a custom frequency
                            frequencyRow.Delete();
                        }
                        else {
                            //edit the notificationfrequency to update it
                            frequencyRow.BeginEdit();
                            frequencyRow.SetField("Time", taskItemDAL.CustomNotificationFrequency.ToString());
                            frequencyRow.EndEdit();
                        }

                    }
                    else if(taskItemDAL.NotificationFrequencyType == "Custom") {
                        //if taskItemDAL has a custom frequency now

                        //create new custom frequency row
                        DataRow frequencyRow = frequencyTable.NewRow();

                        //set information in row
                        try {
                            frequencyRow.SetField("TaskId", taskItemDAL.Id.ToString());
                            frequencyRow.SetField("Time", taskItemDAL.CustomNotificationFrequency.ToString());
                        }
                        catch (Exception ex){
                            frequencyRow.Delete();
                            throw ex;
                        }

                    }

                }
                catch {
                    taskRow.CancelEdit();
                    return false;
                }

                return true;
            }
        }

        public void Dispose() {
            taskAdapter?.Dispose();
            frequencyAdapter?.Dispose();
        }

        public void Save() {
            taskAdapter.Update(taskTable);
            frequencyAdapter.Update(frequencyTable);
        }

        private static TaskItemDAL DataToTaskItemDAL(DataRow taskRow, DataRow frequencyRow = null) {

            TaskItemDAL taskItem = null; 

            if(frequencyRow != null) {
                //handle TaskItem with custom frequency
                taskItem = new TaskItemDAL() {
                    Id = Guid.Parse(taskRow.Field<string>("Id")),
                    StartTime = DateTime.Parse(taskRow.Field<string>("StartTime")),
                    Title = taskRow.Field<string>("Title"),
                    Description = taskRow.Field<string>("Description"),
                    LastNotificationTime = DateTime.Parse(taskRow.Field<string>("LastNotificationTime")),
                    R = (byte)taskRow.Field<long>("R"),
                    G = (byte)taskRow.Field<long>("G"),
                    B = (byte)taskRow.Field<long>("B"),
                    NotificationFrequencyType = taskRow.Field<string>("FrequencyType"),
                    CustomNotificationFrequency = TimeSpan.Parse(frequencyRow.Field<string>("Time"))
                };
            }
            else {
                //TaskItem without custom frequency
                taskItem = new TaskItemDAL() {
                    Id = Guid.Parse(taskRow.Field<string>("Id")),
                    StartTime = DateTime.Parse(taskRow.Field<string>("StartTime")),
                    Title = taskRow.Field<string>("Title"),
                    Description = taskRow.Field<string>("Description"),
                    LastNotificationTime = DateTime.Parse(taskRow.Field<string>("LastNotificationTime")),
                    NotificationFrequencyType = taskRow.Field<string>("FrequencyType"),
                    R = (byte)taskRow.Field<long>("R"),
                    G = (byte)taskRow.Field<long>("G"),
                    B = (byte)taskRow.Field<long>("B")
                };
            }

            return taskItem;
        }

    }
}
