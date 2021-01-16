using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access.Repositories {
    /// <summary>
    /// Defines a class that has a method for producing generic repositories
    /// </summary>
    /// <typeparam name="IRepository">
    /// </typeparam>
    public interface IRepositoryFactory<IRepository> {
        IRepository New();
    }
}
