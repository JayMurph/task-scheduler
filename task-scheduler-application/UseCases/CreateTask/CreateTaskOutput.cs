using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.DTO;

namespace task_scheduler_application.UseCases.CreateTask {
    /// <summary>
    /// Encapsulates the Output of the <see cref="CreateTaskUseCase"/>.
    /// </summary>
    public class CreateTaskOutput : UseCaseOutput{
        /// <summary>
        /// Data-Transfer object representing the new TaskItem created by a <see cref="CreateTaskUseCase"/>
        /// </summary>
        public TaskItemDTO TaskItemDTO; 
    }
}
