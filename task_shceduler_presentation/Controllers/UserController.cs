using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler_application.UseCases.AddTask;

namespace task_scheduler_presentation.Controllers {
    public class UserController {
        public AddTaskUseCaseFactory AddTaskUseCaseFactory;

        public UserController(
            AddTaskUseCaseFactory addTaskUseCaseFactory
            ) {
            this.AddTaskUseCaseFactory = addTaskUseCaseFactory;
        }
    }
}
