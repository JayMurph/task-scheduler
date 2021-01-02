﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler_presentation;

namespace task_scheduler_presentation.Views {
    public interface ITasksView {
        ObservableCollection<TaskItemModel> TaskItems { get; set; } 
    }
}
