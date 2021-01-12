using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access_standard.Repositories {
    public interface IRepository<T> : IDisposable {
        IEnumerable<T> GetAll();
        bool Update(T t);
        bool Add(T t);
        bool Delete(T t);
        void Save();
    }
}
