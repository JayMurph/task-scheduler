﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using task_scheduler_data_access.DataObjects;
using task_scheduler_utility;

namespace task_scheduler_data_access.Repositories {
    class NotificationFrequencyRepository : INotificationFrequencyRepository {

        private readonly DataTable table;
        private readonly SQLiteDataAdapter adapter;

        private bool disposedValue;

        public NotificationFrequencyRepository(string connStr) {
            adapter = NewNotificationFrequencyAdapter(connStr);

            table = new DataTable("Frequencies");
            adapter.FillSchema(table, SchemaType.Source);
            adapter.Fill(table);
        }

        public bool Add(CustomNotificationFrequencyDAL notificationFrequency) {

            //get a new DataRow with the NotificationFrequency table schema
            DataRow newRow = table.NewRow();

            try {
                //set the revelant fields of the new NotificationFrequency row
                newRow.SetField("TaskId", notificationFrequency.taskId.ToString());
                newRow.SetField("Time", notificationFrequency.time.ToString());
            }
            catch {
                //delete the new row
                newRow.Delete();
                return false;
            }

            //add the new row to the table
            table.Rows.Add(newRow);

            return true;
        }

        public bool Delete(CustomNotificationFrequencyDAL notificationFrequency) {
            return Delete(notificationFrequency.taskId);
        }

        public bool Delete(Guid id) {

            var findByIdQuery = GetQueryForId(id);

            if(findByIdQuery.Count() != 1) {
                //row with a matching ID was not found
                return false;
            }
            else {
                //delete the found row
                findByIdQuery.First().Delete();
                return true;
            }
        }

        public IEnumerable<CustomNotificationFrequencyDAL> GetAll() {
            List<CustomNotificationFrequencyDAL> notificationFrequencies = new List<CustomNotificationFrequencyDAL>();

            foreach(DataRow row in table.AsEnumerable()) {
                notificationFrequencies.Add(DataRowToNotificationFrequencyDAL(row));
            }

            return notificationFrequencies;
        }

        public Maybe<CustomNotificationFrequencyDAL> GetById(Guid id) {
            var findByIdQuery = GetQueryForId(id);

            if(findByIdQuery.Count() != 1) {
                return Maybe<CustomNotificationFrequencyDAL>.CreateEmpty();
            }
            else {
                return 
                    Maybe<CustomNotificationFrequencyDAL>.Create(
                        DataRowToNotificationFrequencyDAL(findByIdQuery.First())
                    );
            }
        }

        public bool Save() {
            try {
                adapter.Update(table);
            }
            catch {
                return false;
            }
            return true;
        }

        public bool Update(CustomNotificationFrequencyDAL notificationFrequency) {

            var findByIdQuery = GetQueryForId(notificationFrequency.taskId);

            if(findByIdQuery.Count() != 1) {
                return false;
            }

            DataRow rowToUpdate = findByIdQuery.First();

            try {
                rowToUpdate.BeginEdit();
                rowToUpdate.SetField("Time", notificationFrequency.time.ToString());
                rowToUpdate.EndEdit();
            }
            catch {
                rowToUpdate.CancelEdit();
                return false;
            }

            return true;
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

        private IEnumerable<DataRow> GetQueryForId(Guid id) {
            var findIdQuery = (from row in table.AsEnumerable()
                    where row.Field<string>("TaskId") == id.ToString()
                    select row);
            return findIdQuery;
        }

        private CustomNotificationFrequencyDAL DataRowToNotificationFrequencyDAL(DataRow row) {
            return new CustomNotificationFrequencyDAL(
                Guid.Parse(row.Field<string>("TaskId")),
                TimeSpan.Parse(row.Field<string>("Time"))
            );
        }

        #region Dispose Implementation and Finalizer
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects)
                    table.Dispose();
                    adapter.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~NotificationFrequencyRepository()
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
