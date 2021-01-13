using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.UseCases {
    /// <summary>
    /// Encapsulates the fundamental properties required for the output produced by a Use-Case. 
    /// </summary>
    public abstract class UseCaseOutput {
        /// <summary>
        /// Indicates whether or not the Use-Case that produced the output was executed successfuly
        /// </summary>
        public virtual bool Success { get; set; } = false;
        /// <summary>
        /// Description of any errors that occured during the execution of a Use-Case
        /// </summary>
        public virtual string Error { get; set; } = string.Empty; 
    }
}
