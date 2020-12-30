using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.AddTask {
    public class AddTaskUseCase : IUseCase<AddTaskInput, AddTaskOutput> {

        ITaskManager taskManager;
        INotificationManager notificationManager;
        //task repo
        IClock clock;

        public AddTaskInput Input { set; private get; } = null;

        public AddTaskOutput Output { get; private set; } = null;

        public void Execute() {
            throw new NotImplementedException();

            //retrieve input data
            //create new TaskItem from input data
            //add task to task manager, check for errors
            //create DA TaskItem from new TaskItem
            //add task to task repo, check for errors

            //fill out output data and return
        }
    }
}
