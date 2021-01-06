using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler_data_access.Repositories {
    public interface IRepository<T> {
        //this would be nice if I knew how to implement filter and where expressions as parameters
        //IEnumerable<T> Select();
        IEnumerable<T> GetAll();
        T GetById(object id);
        bool Update(T t);
        bool Add(T t);
        bool Delete(T t);
        bool Delete(object id);
    }
}
