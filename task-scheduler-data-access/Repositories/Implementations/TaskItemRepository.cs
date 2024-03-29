﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using task_scheduler_data_access.DataObjects;
using task_scheduler_utility;

namespace task_scheduler_data_access.Repositories {

    /// <summary>
    /// Repository that manages <see cref="TaskItemDAL"/>s
    /// </summary>
    public class TaskItemRepository : ITaskItemRepository{

        /// <summary>
        /// Holds TaskItem data retrieved from database as DataRows
        /// </summary>
        private readonly DataTable table;

        /// <summary>
        /// Interface to the TaskItem database
        /// </summary>
        private readonly SQLiteDataAdapter adapter;

        /// <summary>
        /// Produces <see cref="NotificationFrequencyRepository"/>s, to allow retrieving Custom
        /// Notification Frequencies that may be associated with TaskItem
        /// </summary>
        private readonly INotificationFrequencyRepository notificationFrequencyRepository;

        private bool disposedValue;

        //TODO: Get rid of all the magic strings in this class
        public TaskItemRepository(string connStr, INotificationFrequencyRepositoryFactory notificationFrequencyRepositoryFactory) {
            adapter = NewTaskItemAdapter(connStr);

            table = new DataTable("Tasks");
            adapter.FillSchema(table, SchemaType.Source);
            adapter.Fill(table);

            notificationFrequencyRepository = notificationFrequencyRepositoryFactory.New();
        }

        /// <summary>
        /// Creates and returns an SQLiteDataAdapter which is connected to a database containing
        /// TaskItem data, and has Select, Insert, Update and Delete commands.
        /// </summary>
        /// <param name="connectionStr">
        /// Used to initialize an SQL database connection
        /// </param>
        /// <returns>
        /// SQLiteDataAdapter initialized with a connection to a database containing TaskItem data,
        /// as well as Select, Insert, Update and Delete commands.
        /// </returns>
        private static SQLiteDataAdapter NewTaskItemAdapter(string connectionStr) {
            SQLiteConnection conn = new SQLiteConnection(connectionStr);

            SQLiteDataAdapter taskAdapter = new SQLiteDataAdapter("SELECT * FROM Tasks", conn)
            {
                InsertCommand =
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
                )
            };

            taskAdapter.InsertCommand.Parameters.Add("@id", DbType.String, 1, "Id");
            taskAdapter.InsertCommand.Parameters.Add("@startTime", DbType.String, 1, "StartTime");
            taskAdapter.InsertCommand.Parameters.Add("@title", DbType.String, 1, "Title");
            taskAdapter.InsertCommand.Parameters.Add("@description", DbType.String, 1, "Description");
            taskAdapter.InsertCommand.Parameters.Add("@lastNotificationTime", DbType.String, 1, "LastNotificationTime");
            taskAdapter.InsertCommand.Parameters.Add("@frequencyType", DbType.Int64, 1, "FrequencyType");
            taskAdapter.InsertCommand.Parameters.Add("@r", DbType.Int64, 1, "R");
            taskAdapter.InsertCommand.Parameters.Add("@g", DbType.Int64, 1, "G");
            taskAdapter.InsertCommand.Parameters.Add("@b", DbType.Int64, 1, "B");

            taskAdapter.DeleteCommand = 
                new SQLiteCommand("DELETE FROM Tasks WHERE Id=@id", conn);
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
            taskAdapter.UpdateCommand.Parameters.Add("@frequencyType", DbType.Int64, 1, "FrequencyType");
            taskAdapter.UpdateCommand.Parameters.Add("@r", DbType.Int64, 1, "R");
            taskAdapter.UpdateCommand.Parameters.Add("@g", DbType.Int64, 1, "G");
            taskAdapter.UpdateCommand.Parameters.Add("@b", DbType.Int64, 1, "B");

            return taskAdapter;
        }

        /// <summary>
        /// Adds a new TaskItemDAL to the repository for it to manage
        /// </summary>
        /// <param name="taskItemDAL">
        /// To be added to the repository
        /// </param>
        /// <returns>
        /// True if the TaskItemDAL was successfuly added to the repository, otherwise false
        /// </returns>
        public bool Add(TaskItemDAL taskItemDAL) {

            //create new row 
            DataRow newTaskRow = table.NewRow();

            if(taskItemDAL.customNotificationFrequency.HasValue) {
                CustomNotificationFrequencyDAL notificationFrequency = taskItemDAL.customNotificationFrequency.Value;

                //add custom notification frequency to notification frequency
                //repository
                if(notificationFrequencyRepository.Add(notificationFrequency) == false) {
                    newTaskRow.Delete();
                    return false;
                }
            }

            try {
                //set all fields of row of new taskItem row
                newTaskRow.SetField("Id", taskItemDAL.id.ToString());
                newTaskRow.SetField("StartTime", taskItemDAL.startTime.ToString());
                newTaskRow.SetField("Title", taskItemDAL.title);
                newTaskRow.SetField("Description", taskItemDAL.description);
                newTaskRow.SetField("LastNotificationTime", taskItemDAL.lastNotificationTime.ToString());
                newTaskRow.SetField("FrequencyType", taskItemDAL.notificationFrequencyType);
                newTaskRow.SetField("R", taskItemDAL.r);
                newTaskRow.SetField("G", taskItemDAL.g);
                newTaskRow.SetField("B", taskItemDAL.b);
            }
            catch {
                //delete the new row since data could not be added
                newTaskRow.Delete();

                if(taskItemDAL.customNotificationFrequency.HasValue) {
                    CustomNotificationFrequencyDAL notificationFrequency = taskItemDAL.customNotificationFrequency.Value;

                    //remove previously added Custom notification frequency
                    notificationFrequencyRepository.Delete(notificationFrequency);
                }
                
                return false;
            }

            table.Rows.Add(newTaskRow);

            return true;
        }

        /// <summary>
        /// Removes a TaskItemDAL from the repository
        /// </summary>
        /// <param name="taskItemDAL">
        /// To be removed from the repository
        /// </param>
        /// <returns>
        /// True if the TaskItemDAL argument was found and removed from the repository, otherwise
        /// false.
        /// </returns>
        public bool Delete(TaskItemDAL taskItemDAL) {
            return Delete(taskItemDAL.id);
        }
        

        /// <param name="taskItemDAL">
        /// To be removed from the repository
        /// </param>
        /// <returns>
        /// True if the TaskItemDAL argument was found and removed from the repository, otherwise
        /// false.
        /// </returns>
        /// <summary>
        /// Removes a TaskItemDAL with an Id corresponding to the id parameter from the repository
        /// </summary>
        /// <param name="id">
        /// Unique id of the TaskItem to be removed from the repository
        /// </param>
        /// <returns>
        /// True if a TaskItem with an Id corresponding to the id parameter was found and removed
        /// from the repository, otherwise false.
        /// </returns>
        public bool Delete(Guid id) {

            //find the item to delete
            var findTaskQuery = GetQueryForId(id);

            //ensure we only found 1 item
            if(findTaskQuery.Count() == 1) {

                //check if there is a custom notification frequency associated with the
                //TaskItem being deleted
                if(notificationFrequencyRepository.GetById(id).HasValue) {

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

        /// <summary>
        /// Retrieves all the TaskItemDALs being managed by the repository
        /// </summary>
        /// <returns>
        /// All the TaskItemDALs being managed by the repository
        /// </returns>
        public IEnumerable<TaskItemDAL> GetAll() {

            List<TaskItemDAL> taskItems = new List<TaskItemDAL>();

            foreach(DataRow taskRow in table.AsEnumerable()) {

                //get custom notification frequency that may be associated with
                //task item
                Guid taskId = Guid.Parse(taskRow.Field<string>("Id"));
                Maybe<CustomNotificationFrequencyDAL> maybeNotificationFrequency = notificationFrequencyRepository.GetById(taskId);

                taskItems.Add(DataToTaskItemDAL(taskRow, maybeNotificationFrequency)); 
            }

            return taskItems;
        }

        /// <summary>
        /// Retrieves a TaskItemDAL with an Id corresponding to the id parameter from the repository 
        /// </summary>
        /// <param name="id">
        /// Unique Id of the TaskItem to retrieve
        /// </param>
        /// <returns>
        /// </returns>
        public Maybe<TaskItemDAL> GetById(Guid id) {
            var findTaskQuery = GetQueryForId(id);

            if(findTaskQuery.Count() != 1) {
                return Maybe<TaskItemDAL>.CreateEmpty();
            }
            else {

                //get the custom notification frequency that may be associated with
                //the TaskItem
                Maybe<CustomNotificationFrequencyDAL> maybeNotificationFrequency =
                    notificationFrequencyRepository.GetById(id);

                return Maybe<TaskItemDAL>.Create(
                    DataToTaskItemDAL(findTaskQuery.First(), maybeNotificationFrequency)
                );
            }
        }

        /// <summary>
        /// Finds and updates the TaskItemDAL in the repository with the info of the TaskItemDAL
        /// being passed in as a parameter. The taskItemDAL parameter is expected to have an Id 
        /// matching that of an item already present in the repository.
        /// </summary>
        /// <param name="taskItemDAL">
        /// Contains the Id of a TaskItemDAL that exists in the repository and new information to
        /// update the existing TaskItemDAL with.
        /// </param>
        /// <returns>
        /// True if a TaskItem with an Id matching that of the taskItemDAL could be found in the
        /// repository and if its fields were successfuly updated with the incoming data, otherwise
        /// false.
        /// </returns>
        public bool Update(TaskItemDAL taskItemDAL) {
            var findTaskQuery = GetQueryForId(taskItemDAL.id);

            if(findTaskQuery.Count() != 1) {
                return false;
            }
            else {
                DataRow taskRow = findTaskQuery.First();

                try {
                    taskRow.BeginEdit();
                    taskRow.SetField("StartTime", taskItemDAL.startTime.ToString());
                    taskRow.SetField("Title", taskItemDAL.title);
                    taskRow.SetField("Description", taskItemDAL.description);
                    taskRow.SetField("LastNotificationTime", taskItemDAL.lastNotificationTime.ToString());
                    taskRow.SetField("FrequencyType", taskItemDAL.notificationFrequencyType);
                    taskRow.SetField("R", taskItemDAL.r);
                    taskRow.SetField("G", taskItemDAL.g);
                    taskRow.SetField("B", taskItemDAL.b);
                    taskRow.EndEdit();

                    //get the custom notification frequency that may be associated with
                    //the TaskItem, from the database
                    Maybe<CustomNotificationFrequencyDAL> maybeNotificationFrequency =
                        notificationFrequencyRepository.GetById(taskItemDAL.id);

                    if(maybeNotificationFrequency.HasValue) {
                        //if a custom frequency already exists for this taskitem

                        if(taskItemDAL.customNotificationFrequency.HasValue) {
                            //update the custom notification frequency
                            //TODO: check that update is successful/handle failure
                            notificationFrequencyRepository.Update(taskItemDAL.customNotificationFrequency.Value);
                        }
                        else {
                            //TaskItem no longer has a custom notification frequency
                            //TODO: check that delete is successful/handle failure
                            notificationFrequencyRepository.Delete(maybeNotificationFrequency.Value);
                        }

                    }
                    else if(taskItemDAL.customNotificationFrequency.HasValue ) {
                        //if taskItemDAL has a custom frequency now, then add it
                        //to the NotificationRepository
                        //TODO: check for success and handle failure
                        notificationFrequencyRepository.Add(taskItemDAL.customNotificationFrequency.Value);
                    }

                }
                catch {
                    taskRow.CancelEdit();
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Saves and persists all changed made to TaskItems in the current repository instance since the last time
        /// Save was called or since the repository was constructed.
        /// </summary>
        /// <returns>
        /// True if the changes made to the repository could be saved, otherwise false
        /// </returns>
        public bool Save() {
            try {
                adapter.Update(table);

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

        /// <summary>
        /// Converts the data in the fields of a DataRow into a TaskItemDAL. 
        /// </summary>
        /// <param name="taskRow">
        /// Contains fields of TaskItem data
        /// </param>
        /// <param name="customNotificationFrequency">
        /// To be assigned to the CustomNotificationFrequency field of the
        /// TaskItemDAL being created.
        /// </param>
        /// <returns>
        /// TaskItemDAL containing the TaskItem data extracted from the taskRow parameter and the
        /// customNotificationFrequency
        /// </returns>
        private static TaskItemDAL DataToTaskItemDAL(DataRow taskRow, Maybe<CustomNotificationFrequencyDAL> customNotificationFrequency) {
            /*
             * TODO: perform a sanity check on the fields of the incoming DataRow; ensure that the
             * fields we want to access are present
             */
            TaskItemDAL taskItem = new TaskItemDAL(
                id : Guid.Parse(taskRow.Field<string>("Id")),
                startTime : DateTime.Parse(taskRow.Field<string>("StartTime")),
                title : taskRow.Field<string>("Title"),
                description : taskRow.Field<string>("Description"),
                lastNotificationTime : DateTime.Parse(taskRow.Field<string>("LastNotificationTime")),
                r : (byte)taskRow.Field<long>("R"),
                g : (byte)taskRow.Field<long>("G"),
                b : (byte)taskRow.Field<long>("B"),
                notificationFrequencyType : (int)taskRow.Field<long>("FrequencyType"),
                customNotificationFrequency : customNotificationFrequency
            );

            return taskItem;
        }

        /// <summary>
        /// Returns an IEnumerable formed from a query for a specific ID in the Tasks table.
        /// </summary>
        /// <param name="id">
        /// Id of a Task to find
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<DataRow> GetQueryForId(Guid id) {
            var findIdQuery = (from row in table.AsEnumerable()
                    where row.Field<string>("Id") == id.ToString()
                    select row);

            return findIdQuery;
        }

        #region Dispose Implementation and Finalizer
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects)
                    adapter.Dispose();
                    table.Dispose();
                    notificationFrequencyRepository.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TaskItemRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
