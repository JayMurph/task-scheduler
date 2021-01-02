using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_data_access {
    public class TaskItemUnitOfWork {
        TaskItemsContext context = new TaskItemsContext();
        TaskItemDALRepository repository;

        public TaskItemUnitOfWork() {
            this.repository = new TaskItemDALRepository(context); 
        }

        public TaskItemDALRepository Repository { get { return this.repository ?? new TaskItemDALRepository(context); } }

        public void Save() {
            context.SaveChanges();
        }
    }
}
