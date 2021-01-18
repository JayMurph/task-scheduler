using System;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Text;
using task_scheduler_data_access.DataObjects;

namespace task_scheduler_data_access.Repositories {
    /// <summary>
    /// Provides a data-store of Notifications. This Repository does not support updating, due to
    /// the nature of Notifications.
    /// </summary>
    public class NotificationRepository : INotificationRepository{

        private SQLiteDataAdapter adapter;

        private DataTable table;

        private bool disposedValue;

        public NotificationRepository(string connectionStr) {

            adapter = CreateNotificationTableAdapter(connectionStr);

            table = new DataTable("Notifications");

            adapter.FillSchema(table, SchemaType.Source);
            adapter.Fill(table);
        }

        /// <summary>
        /// Returns an SQLiteDataAdapter configured to Insert, Select and Delete entries
        /// from a Notifications table
        /// </summary>
        /// <param name="connectionString">
        /// To be used by the new SQLiteDataAdapter's Connection property
        /// </param>
        /// <returns>
        /// SQLiteDataAdapter configured to interact with a Notifications table
        /// </returns>
        private static SQLiteDataAdapter CreateNotificationTableAdapter(string connectionString) {

            if (string.IsNullOrWhiteSpace(connectionString)) {
                throw new ArgumentException($"'{nameof(connectionString)}' cannot be null or whitespace", nameof(connectionString));
            }

            SQLiteConnection conn = new SQLiteConnection(connectionString);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Notifications", conn);

            adapter.InsertCommand = new SQLiteCommand("INSERT INTO Notifications VALUES(@id, @time)");
            adapter.InsertCommand.Parameters.Add("@id", DbType.String, 1, "TaskId");
            adapter.InsertCommand.Parameters.Add("@time", DbType.String, 1, "Time");

            /*
                adapter.UpdateCommand = new SQLiteCommand("UPDATE Notifications SET Time=@time WHERE TaskId=@id");
                adapter.UpdateCommand.Parameters.Add("@id", DbType.String, 1, "TaskId");
                adapter.UpdateCommand.Parameters.Add("@time", DbType.String, 1, "Time");
            */

            adapter.DeleteCommand = new SQLiteCommand("DELETE FROM Notifications WHERE TaskId=@id AND Time=@time");
            adapter.DeleteCommand.Parameters.Add("@id", DbType.String, 1, "TaskId");
            adapter.DeleteCommand.Parameters.Add("@time", DbType.String, 1, "Time");

            return adapter;
        }

        /// <summary>
        /// Adds a new Notification to the repository for it to manage.
        /// </summary>
        /// <param name="notification">
        /// To be added to the repository
        /// </param>
        /// <returns>
        /// True if the NotificationDAL was successfuly added to the repository, otherwise false.
        /// </returns>
        public bool Add(NotificationDAL notification) {
            //create new row with table's schema
            DataRow newRow = table.NewRow();

            try {
                //set the fields of the new row
                newRow.SetField("TaskId", notification.taskId.ToString());
                newRow.SetField("Time", notification.time.ToString());
            }
            catch {
                newRow.Delete();
                return false;
            }

            //add the row to the table
            table.Rows.Add(newRow);

            return true;
        }

        /// <summary>
        /// Removes a Notification from the repository
        /// </summary>
        /// <param name="notification">
        /// To be removed from the repository
        /// </param>
        /// <returns>
        /// True if the Notification was found and removed from the repository, otherwise false
        /// </returns>
        public bool Delete(NotificationDAL notification) {
            /*
             * find the Notification in the repo. It is identified by the combination of its TaskId
             * and Time fields
             */
            var findQuery = from row in table.AsEnumerable()
                            where row.Field<string>("TaskId") == notification.taskId.ToString() &&
                            row.Field<string>("Time") == notification.time.ToString()
                            select row;

            if(findQuery.Count() != 1) {
                return false;
            }

            try {
                findQuery.First().Delete();
            }
            catch {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns all the NotificationDALs managed by the Repository
        /// </summary>
        /// <returns>
        /// All the NotificationDALs managed by the Repository
        /// </returns>
        public IEnumerable<NotificationDAL> GetAll() {

            List<NotificationDAL> notifications = new List<NotificationDAL>();

            /*
             * create NotificationDALs from every row in the Notifications table. Add them to a
             * collection to return
             */
            foreach(DataRow row in table.AsEnumerable()) {
                notifications.Add(
                    new NotificationDAL(
                        Guid.Parse(row.Field<string>("TaskId")),
                        DateTime.Parse(row.Field<string>("Time"))
                    )
                );
            }

            return notifications;
        }

        /// <summary>
        /// Saves all changes made to the NotificationRepository instance since it was
        /// constructured, or since the last time Save was called.
        /// </summary>
        /// <returns>
        /// True if the recent changes made to the repository were successfuly saved, otherwise
        /// false
        /// </returns>
        public bool Save() {
            try {
                adapter.Update(table);
            }
            catch {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Not supported by this repository because all the elements of a Notification
        /// form a composite key, so editing them would affect the Notifications identity
        /// </summary>
        /// <param name="notification">unused</param>
        /// <returns>always false</returns>
        public bool Update(NotificationDAL notification) {
            return false;
        }

        #region Dispose Implementation and Finalizer
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects)
                    adapter.Dispose();
                    table.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~NotificationRepository()
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
