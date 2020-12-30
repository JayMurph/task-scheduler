using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application {
    /// <summary>
    /// Interface for classes that interact with a database 
    /// </summary>
    /// <typeparam name="T">Type that is created, updated, retrieved, and deleted from a
    /// data-source</typeparam>
    public interface IRepository<T>{
        bool Insert(T t);
        IEnumerable<T> Select(Predicate<T> predicate);
        bool Update(T t);
        bool Delete(T t);
    }
}
