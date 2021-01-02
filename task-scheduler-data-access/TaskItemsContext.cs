using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace task_scheduler_data_access {
    public class TaskItemsContext : DbContext {
        public DbSet<TaskItemDAL> TaskItems { get; set; }
        public DbSet<NotificationFrequencyDAL> NotificationFrequencies {get;set;}
    }
}
