using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler_data_access.DataObjects;

namespace task_scheduler_data_access.Repositories {
    public interface ITaskItemRepository : IRepository<TaskItemDAL>{
    }
}
