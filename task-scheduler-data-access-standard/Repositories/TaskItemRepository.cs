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
        private readonly SQLiteDataAdapter taskAdapter;

        private readonly INotificationFrequencyRepository notificationFrequencyRepository;
        public TaskItemRepository(string connStr, INotificationFrequencyRepositoryFactory notificationFrequencyRepositoryFactory) {

            taskAdapter = NewTaskItemAdapter(connStr);

            table = new DataTable("Tasks");
            taskAdapter.FillSchema(table, SchemaType.Source);
            taskAdapter.Fill(table);

            notificationFrequencyRepository = notificationFrequencyRepositoryFactory.New();
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
            DataRow newTaskRow = table.NewRow();

            if(taskItemDAL.CustomNotificationFrequency != null) {
                //add custom notification frequency to notification frequency
                //repository
                if(notificationFrequencyRepository.Add(taskItemDAL.CustomNotificationFrequency) ==
                    false) {
                    newTaskRow.Delete();
                    return false;
                }
            }

            try {
                //set all fields of row of new taskItem row
                newTaskRow.SetField("Id", taskItemDAL.Id.ToString());
                newTaskRow.SetField("StartTime", taskItemDAL.StartTime.ToString());
                newTaskRow.SetField("Title", taskItemDAL.Title);
                newTaskRow.SetField("Description", taskItemDAL.Description);
                newTaskRow.SetField("LastNotificationTime", taskItemDAL.LastNotificationTime.ToString());
                newTaskRow.SetField("FrequencyType", taskItemDAL.NotificationFrequencyType);
                newTaskRow.SetField("R", taskItemDAL.R);
                newTaskRow.SetField("G", taskItemDAL.G);
                newTaskRow.SetField("B", taskItemDAL.B);
            }
            catch {
                //delete the new row since data could not be added
                newTaskRow.Delete();

                if(taskItemDAL.CustomNotificationFrequency != null) {
                    //remove previously added Custom notification frequency
                    notificationFrequencyRepository.Delete(taskItemDAL.CustomNotificationFrequency);
                }
                
                return false;
            }

            table.Rows.Add(newTaskRow);

            return true;
        }

        public bool Delete(TaskItemDAL taskItemDAL) {
            return Delete(taskItemDAL.Id);
        }
        

        public bool Delete(Guid id) {

            //find the item to delete
            var findTaskQuery = GetQueryForId(id);

            //ensure we only found 1 item
            if(findTaskQuery.Count() == 1) {

                //check if there is a custom notification frequency associated with the
                //TaskItem being deleted
                if(notificationFrequencyRepository.GetById(id) != null) {
                    //delete TaskItem's custom notification frequency
                    if(notificationFrequencyRepository.Delete(id) == false) {
                        //return false if unable to delete custom notification frequency
                        return false;
                    }
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

            foreach(DataRow taskRow in table.AsEnumerable()) {

                //get custom notification frequency that may be associated with
                //task item
                Guid taskId = Guid.Parse(taskRow.Field<string>("Id"));
                NotificationFrequencyDAL notificationFrequency = notificationFrequencyRepository.GetById(taskId);

                if(notificationFrequency == null) {
                    taskItems.Add(DataToTaskItemDAL(taskRow)); 
                }
                else {
                    taskItems.Add(DataToTaskItemDAL(taskRow, notificationFrequency)); 
                }
            }

            return taskItems;
        }

        public TaskItemDAL GetById(Guid id) {
            var findTaskQuery = GetQueryForId(id);

            if(findTaskQuery.Count() != 1) {
                return null;
            }
            else {

                //get the custom notification frequency that may be associated with
                //the TaskItem
                NotificationFrequencyDAL notificationFrequency =
                    notificationFrequencyRepository.GetById(id);

                if(notificationFrequency == null) {
                    return DataToTaskItemDAL(findTaskQuery.First());
                }
                else {
                    return DataToTaskItemDAL(findTaskQuery.First(), notificationFrequency); 
                }
            }
        }

        public bool Update(TaskItemDAL taskItemDAL) {
            var findTaskQuery = GetQueryForId(taskItemDAL.Id);

            if(findTaskQuery.Count() != 1) {
                return false;
            }
            else {
                DataRow taskRow = findTaskQuery.First();

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

                    //get the custom notification frequency that may be associated with
                    //the TaskItem, from the database
                    NotificationFrequencyDAL notificationFrequency =
                        notificationFrequencyRepository.GetById(taskItemDAL.Id);

                    if(notificationFrequency != null) {
                        //if a custom frequency already exists for this taskitem

                        if(taskItemDAL.NotificationFrequencyType != "Custom") {
                            //TaskItem no longer has a custom notification frequency
                            //TODO: check that delete is successful/handle failure
                            notificationFrequencyRepository.Delete(notificationFrequency);
                        }
                        else {
                            //update the custom notification frequency
                            //TODO: check that update is successful/handle failure
                            notificationFrequencyRepository.Update(taskItemDAL.CustomNotificationFrequency);
                        }

                    }
                    else if(taskItemDAL.NotificationFrequencyType == "Custom") {
                        //if taskItemDAL has a custom frequency now, then add it
                        //to the NotificationRepository
                        //TODO: check for success and handle failure
                        notificationFrequencyRepository.Add(taskItemDAL.CustomNotificationFrequency);
                    }

                }
                catch {
                    taskRow.CancelEdit();
                    return false;
                }

                return true;
            }
        }

        //TODO : Implement dispose properly 
        public void Dispose() {
            taskAdapter?.Dispose();
            notificationFrequencyRepository.Dispose();
        }

        public bool Save() {
            try {
                taskAdapter.Update(table);

                //TODO: need to figure out a way to undo task table changes
                //if the following fails
                if (!notificationFrequencyRepository.Save()) {
                    return false;
                }
            }
            catch {
                return false;
            }

            return true;
        }

        private static TaskItemDAL DataToTaskItemDAL(DataRow taskRow, NotificationFrequencyDAL notificationFrequency = null) {

            TaskItemDAL taskItem = null; 

            if(notificationFrequency != null) {
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
                    CustomNotificationFrequency = notificationFrequency
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

        private IEnumerable<DataRow> GetQueryForId(Guid id) {
            var findIdQuery = (from row in table.AsEnumerable()
                    where row.Field<string>("Id") == id.ToString()
                    select row);

            return findIdQuery;
        }
    }
}
