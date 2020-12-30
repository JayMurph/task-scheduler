using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.AddTask {
    class AddTaskInput {
        public string Title;
        public string Comment;
        public DateTime StartTime;
        public Colour Colour;
        //PERIOD representation
    }
}
