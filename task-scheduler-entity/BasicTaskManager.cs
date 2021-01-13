using System;
using System.Collections.Generic;
using System.Text;

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

        public bool Remove(ITaskItem taskItem) {
            if (tasks.Remove(taskItem)) {
                taskItem.Dispose();
                return true;
            }
            return false;
        }
    }
}
