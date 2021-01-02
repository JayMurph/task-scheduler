using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.DTO;

namespace task_scheduler_application.UseCases.ViewTasks {
    public class ViewTasksOutput : UseCaseOutput{
        public List<TaskItemDTO> TaskItems { get; set; } = new List<TaskItemDTO>();
        //does this need a success property, or error text????????
    }
}
