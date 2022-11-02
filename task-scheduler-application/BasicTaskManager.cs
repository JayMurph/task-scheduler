using System;
using System.Collections.Generic;
using System.Linq;

namespace task_scheduler_entities {

    /// <summary>
    /// Basic implementation of the ITaskManager interface. 
    /// </summary>
    public class BasicTaskManager : ITaskManager {

        private readonly List<ITaskItem> tasks = new List<ITaskItem>();

        /// <summary>
        /// Gives a new ITaskItem to the BasicTaskManager for it to maintain.
        /// </summary>
        /// <param name="taskItem">
        /// To be added to the BasicTaskManager's collection of ITaskItems
        /// </param>
        /// <returns>
        /// True if the given taskItem was successfully added to the
        /// BasicTaskManager's collection, otherwise false.
        /// </returns>
        public bool Add(ITaskItem taskItem) {
            if (taskItem != null && !tasks.Contains(taskItem)) {
                tasks.Add(taskItem);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns all the Tasks managed by the BasicTaskManager
        /// </summary>
        /// <returns></returns>
        public List<ITaskItem> GetAll() {
            return new List<ITaskItem>(tasks);
        }

        /// <summary>
        /// Retrieves the ITaskItem with the corresponding id
        /// </summary>
        /// <param name="id">
        /// Uniquer identifier of an ITaskItem
        /// </param>
        /// <returns>
        /// ITaskItem with an id corresponding to the id parameter. null if an
        /// ITaskItem with the same id could not be found.
        /// </returns>
        public ITaskItem Find(Guid id) {
            var matches = tasks.Where(x => x.ID == id);

            if(matches.Count() != 1) {
                return null;
            }

            return matches.First();
        }

        /// <summary>
        /// Removes the given taskItem from the collection of ITaskItems
        /// managed by the BasicTaskManager and disposes of the ITaskItem
        /// </summary>
        /// <param name="taskItem">
        /// To be removed from the BasicTaskManager's collection and disposed
        /// of.
        /// </param>
        /// <returns>
        /// True if the taskItem was removed from the BasicTaskManager's
        /// collection and disposed of, otherwise false.
        /// </returns>
        public bool Remove(ITaskItem taskItem) {
            if (taskItem == null)
            {
                throw new ArgumentNullException($"Parameter {nameof(taskItem)} is NULL");
            }

            if (tasks.Remove(taskItem)) {
                taskItem.Dispose();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the ITaskItem with the given id from the collection of
        /// ITaskItems managed by the BasicTaskManager and disposes of it.
        /// </summary>
        /// <param name="id">
        /// Id of an ITaskItem managed by the BasicTaskManager that will be
        /// removed from its collection then disposed of.
        /// </param>
        /// <returns>
        /// True if the ITaskItem with the provided id was removed from the
        /// BasicTaskManager's collection and disposed of, otherwise false.
        /// </returns>
        public bool Remove(Guid id) {
            ITaskItem taskToRemove = Find(id);
            if(taskToRemove != null) {
                return Remove(taskToRemove);
            }
            else {
                return false;
            }
        }
    }
}
