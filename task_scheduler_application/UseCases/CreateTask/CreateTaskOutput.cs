using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.DTO;

namespace task_scheduler_application.UseCases.CreateTask {
    public class CreateTaskOutput {
        public bool Success;
        public TaskItemDTO TaskItemDTO; 
        public string Error;
    }
}
