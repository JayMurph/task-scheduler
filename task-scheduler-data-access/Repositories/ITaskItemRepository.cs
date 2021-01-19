using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler_data_access.DataObjects;
using task_scheduler_utility;

namespace task_scheduler_data_access.Repositories {
    /// <summary>
    /// Defines the functionality required for an IRepository that manages TaskItemDAL's
    /// </summary>
    public interface ITaskItemRepository : IRepository<TaskItemDAL>{

        /// <summary>
        /// Retrieves a TaskItemDAL with an Id corresponding to the id parameter
        /// </summary>
        /// <param name="id">
        /// Unique id of the TaskItem to retrieve
        /// </param>
        /// <returns>
        /// TaskItemDAL with an Id corresponding to the id parameter. null if no match is found for
        /// the id.
        /// </returns>
        Maybe<TaskItemDAL> GetById(Guid id);

        /// <summary>
        /// Deletes a TaskItemDAL managed by the repository which has an Id corresponding to the id
        /// parameter.
        /// </summary>
        /// <param name="id">
        /// Unique id of the TaskItemDAL to be deleted from the repository
        /// </param>
        /// <returns>
        /// True if the TaskItemDAL identified by the id parameter was successfuly found and
        /// deleted, otherwise false
        /// </returns>
        bool Delete(Guid id);
    }
}
