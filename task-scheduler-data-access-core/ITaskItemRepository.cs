using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler_data_access_core.DataObjects;

namespace task_scheduler_data_access_core.Repositories {
    public interface ITaskItemRepository : IRepository<TaskItemDAL>{
    }
}
