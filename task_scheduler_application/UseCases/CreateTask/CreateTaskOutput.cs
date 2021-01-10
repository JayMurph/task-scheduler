using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.DTO;

namespace task_scheduler_application.UseCases.CreateTask {
    public class CreateTaskOutput : UseCaseOutput{
        public TaskItemDTO TaskItemDTO; 
    }
}
