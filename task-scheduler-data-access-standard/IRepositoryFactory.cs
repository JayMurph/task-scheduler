using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access_standard.Repositories {
    public interface IRepositoryFactory<IRepository> {
        IRepository New();
    }
}
