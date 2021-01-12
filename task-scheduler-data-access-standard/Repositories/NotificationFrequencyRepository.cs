﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_data_access_standard.Repositories {
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

        public bool Add(NotificationFrequencyDAL notificationFrequency) {

            //get a new DataRow with the NotificationFrequency table schema
            DataRow newRow = table.NewRow();

            try {
                //set the revelant fields of the new NotificationFrequency row
                newRow.SetField("TaskId", notificationFrequency.TaskId.ToString());
                newRow.SetField("Time", notificationFrequency.Time.ToString());
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

        public bool Delete(NotificationFrequencyDAL notificationFrequency) {
            return Delete(notificationFrequency.TaskId);
        }

        public bool Delete(object id) {

            var idQuery = GetQueryForId(id);

            if(idQuery.Count() != 1) {
                //row with a matching ID was not found
                return false;
            }
            else {
                //delete the found row
                idQuery.First().Delete();
                return true;
            }
        }

        public IEnumerable<NotificationFrequencyDAL> GetAll() {
            List<NotificationFrequencyDAL> notificationFrequencies = new List<NotificationFrequencyDAL>();

            foreach(DataRow row in table.AsEnumerable()) {
                notificationFrequencies.Add(DataRowToNotificationFrequencyDAL(row));
            }

            return notificationFrequencies;
        }

        public NotificationFrequencyDAL GetById(object id) {
            var findByIdQuery = GetQueryForId(id);

            if(findByIdQuery.Count() != 1) {
                return null;
            }
            else {
                return DataRowToNotificationFrequencyDAL(findByIdQuery.First());
            }
        }

        public void Save() {
            adapter.Update(table);
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

        private IEnumerable<DataRow> GetQueryForId(object id) {
            var findIdQuery = (from row in table.AsEnumerable()
                    where row.Field<string>("TaskId") == id.ToString()
                    select row);
            return findIdQuery;
        }

        private NotificationFrequencyDAL DataRowToNotificationFrequencyDAL(DataRow row) {
            return new NotificationFrequencyDAL(
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
