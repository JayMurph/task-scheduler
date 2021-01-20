using System.Collections.Generic;

namespace task_scheduler_entities {
    /// <summary>
    /// Interface for classes that manage a collection of unique ITaskItems
    /// </summary>
    public interface ITaskManager {
        /// <summary>
        /// Adds a new ITaskItem to the ITaskManagers collection
        /// </summary>
        /// <param name="taskItem">
        /// To be added to the ITaskManager's collection
        /// </param>
        /// <returns>
        /// True if the specified ITaskItem could be added to the ITaskManager's collection,
        /// otherwise false
        /// </returns>
        bool Add(ITaskItem taskItem);

        /// <summary>
        /// Removes an ITaskItem from the ITaskManager's collection
        /// </summary>
        /// <param name="taskItem">
        /// To be removed from the ITaskManager's collection
        /// </param>
        /// <returns>
        /// True if the ITaskItem could be removed from the ITaskManager's collection,
        /// otherwise false
        /// </returns>
        bool Remove(ITaskItem taskItem);

        /// <summary>
        /// Retrieves a List containing all the ITaskItem's in the ITaskManager's collection
        /// </summary>
        /// <returns>
        /// Contains all the ITaskItems managed by the ITaskManager
        /// </returns>
        List<ITaskItem> GetAll();
    }
}
