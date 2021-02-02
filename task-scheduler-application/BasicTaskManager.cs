using System;
using System.Collections.Generic;
using System.Linq;

namespace task_scheduler_entities {
    /// <summary>
    /// Basic implementation of the ITaskManager interface. 
    /// </summary>
    public class BasicTaskManager : ITaskManager {

        private List<ITaskItem> tasks = new List<ITaskItem>();

        public bool Add(ITaskItem taskItem) {
            if (!tasks.Contains(taskItem)) {
                tasks.Add(taskItem);
                return true;
            }

            return false;
        }

        public List<ITaskItem> GetAll() {
            return new List<ITaskItem>(tasks);
        }

        public ITaskItem Find(Guid id) {
            var matches = tasks.Where(x => x.ID == id);

            if(matches.Count() != 1) {
                return null;
            }

            return matches.First();
        }

        public bool Remove(ITaskItem taskItem) {
            if (tasks.Remove(taskItem)) {
                taskItem.Dispose();
                return true;
            }
            return false;
        }

        public bool Remove(Guid taskId) {
            ITaskItem taskToRemove = tasks.Where(x => x.ID == taskId).FirstOrDefault();
            if(taskToRemove != null) {
                return tasks.Remove(taskToRemove);
            }
            else {
                return false;
            }
        }
    }
}
