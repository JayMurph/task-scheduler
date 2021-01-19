using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_data_access.DataObjects;
using task_scheduler_utility;

namespace task_scheduler_data_access.Repositories {
    /// <summary>
    /// Defines the functionality required for an IRepository that manages
    /// CustomNotificationFrequencyDALs
    /// </summary>
    public interface INotificationFrequencyRepository : IRepository<CustomNotificationFrequencyDAL> {

        /// <summary>
        /// Retrieves a CustomNotificationFrequencyDAL with an Id corresponding to the id parameter
        /// </summary>
        /// <param name="id">
        /// Unique id of the CustomNotificationFrequency to retrieve
        /// </param>
        /// <returns>
        /// CustomNotificationFrequencyDAL with an Id corresponding to the id parameter. null if no
        /// match is found for the id.
        /// </returns>
        Maybe<CustomNotificationFrequencyDAL> GetById(Guid id);

        /// <summary>
        /// Deletes a CustomNotificationFrequencyDAL managed by the repository which has an Id
        /// corresponding to the id parameter.
        /// </summary>
        /// <param name="id">
        /// Unique id of the CustomNotificationFrequencyDAL to be deleted from the repository
        /// </param>
        /// <returns>
        /// True if the CustomNotificationFrequencyDAL identified by the id parameter was
        /// successfuly found and deleted, otherwise false
        /// </returns>
        bool Delete(Guid id);
    }
}
