using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.UseCases {
    public abstract class UseCaseOutput {
        public virtual bool Success { get; set; } = false;
        public virtual string Error { get; set; } = string.Empty; 
    }
}
