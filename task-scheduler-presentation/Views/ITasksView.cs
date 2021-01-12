using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler_presentation.Models;

namespace task_scheduler_presentation.Views {
    public interface ITasksView {
        ObservableCollection<TaskItemModel> TaskItems { get; set; }
        void TaskCreatedCallback(object source, TaskItemModel taskItemModel);
        event EventHandler Closing;
    }
}
