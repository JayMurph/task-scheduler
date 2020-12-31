using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.AddTask {
    public class AddTaskUseCase : IUseCase<AddTaskInput, AddTaskOutput> {

        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly Repositories.ITaskItemRepository taskRepo;
        private readonly IClock clock;

        public AddTaskUseCase(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            ITaskItemRepository taskRepo,
            IClock clock) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.taskRepo = taskRepo ?? throw new ArgumentNullException(nameof(taskRepo));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

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
