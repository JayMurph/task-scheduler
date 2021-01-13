using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.DTO;

namespace task_scheduler_application.UseCases.ViewTasks {
    /// <summary>
    /// Encapsulates the Output of the <see cref="ViewTasksUseCase"/>
    /// </summary>
    public class ViewTasksOutput : UseCaseOutput{
        public List<TaskItemDTO> TaskItems { get; set; } = new List<TaskItemDTO>();
    }
}
