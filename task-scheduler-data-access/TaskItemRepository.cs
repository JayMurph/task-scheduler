using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace task_scheduler_data_access {
    class TaskItemRepository : ITaskItemRepository, IDisposable{
        private DataTable table;
        private SQLiteDataAdapter adapter;

        public TaskItemRepository(string connStr) {
            adapter = new SQLiteDataAdapter("SELECT * FROM Tasks", new SQLiteConnection(connStr));

            adapter.DeleteCommand = new SQLiteCommand("DELETE FROM Tasks WHERE Id='@id'");

            adapter.UpdateCommand =
                new SQLiteCommand(
                    "UPDATE Tasks SET Id=@id, " +
                    "StartTime=@startTime, " +
                    "Title=@title, " +
                    "Description=@description, " +
                    "LastNotificationTime=@lastNotificationTime, " +
                    "FrequencyType=@frequencyType, " +
                    "R=@r, " +
                    "B=@b, " +
                    "C=@c"
                );

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
                row.SetField("LastNotificationTime", taskItemDAL.LastNotificationTime);
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
            throw new NotImplementedException();
        }

        public void Dispose() {
            adapter?.Dispose();
        }

        public void Save() {
            table.AcceptChanges();
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
                R = byte.Parse(row.Field<string>("R")),
                G = byte.Parse(row.Field<string>("G")),
                B = byte.Parse(row.Field<string>("B"))
            };
        }

    }
}
