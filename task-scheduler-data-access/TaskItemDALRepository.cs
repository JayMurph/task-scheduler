using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access {
    public class TaskItemDALRepository : IRepository<TaskItemDAL> {

        private TaskItemsContext context;

        public TaskItemDALRepository(TaskItemsContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public bool Delete(TaskItemDAL t) {
            context.TaskItems.Remove(t);
            return true;
        }

        public bool Insert(TaskItemDAL t) {
            context.TaskItems.Add(t);
            return true;
        }

        public IEnumerable<TaskItemDAL> Get() {
            return context.TaskItems.ToList();
        }

        public bool Update(TaskItemDAL t) {
            context.TaskItems.Attach(t);
            context.Entry(t).State = System.Data.Entity.EntityState.Modified;
            return true;
        }
    }
}
