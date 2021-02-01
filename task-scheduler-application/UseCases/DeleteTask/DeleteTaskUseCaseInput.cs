using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.UseCases.DeleteTask {
    /// <summary>
    /// Encapsulates the input required for the Delete Task use case
    /// </summary>
    public class DeleteTaskUseCaseInput {
        /// <summary>
        /// The unique identifier of the TaskItem to be deleted
        /// </summary>
        Guid Id;
    }
}
