using System;
using System.Collections.Generic;

namespace task_scheduler_data_access.Repositories {
    /// <summary>
    /// Defines the components necessary for a generic repository. Allows CRUD operations on a
    /// collection of objects.
    /// </summary>
    /// <typeparam name="T">
    /// The type of objects that the repository maintains
    /// </typeparam>
    public interface IRepository<T> : IDisposable {
        /// <summary>
        /// Retrieves all objects managed by the repository
        /// </summary>
        /// <returns>
        /// All objects managed by the repository
        /// </returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Updates the values of an object in the repository with that of the object provided as an
        /// argument.
        /// </summary>
        /// <param name="t">
        /// Contains values to update an in-repository object with
        /// </param>
        /// <returns>
        /// True if the Update operation was successful, otherwise false
        /// </returns>
        bool Update(T t);

        /// <summary>
        /// Adds a new object to the repository for it to manage
        /// </summary>
        /// <param name="t">
        /// Object to be added to an managed by the repository
        /// </param>
        /// <returns>
        /// True if the provided object was successfuly added to the repository, otherwise false
        /// </returns>
        bool Add(T t);

        /// <summary>
        /// Removes the object passed as a parameter from the repository
        /// </summary>
        /// <param name="t">
        /// Object to be deleted in the repository
        /// </param>
        /// <returns>
        /// True if the provided object was successfuly deleted from the repository, otherwise false
        /// </returns>
        bool Delete(T t);

        /// <summary>
        /// Persists all changes made to the objects managed by the repository since the last-time
        /// Save was called.
        /// </summary>
        /// <returns>
        /// True if the changes made to the repositorie's objects were successfuly saved, otherwise false.
        /// </returns>
        bool Save();
    }
}
