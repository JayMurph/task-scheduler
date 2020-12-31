using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.CreateTask {
    public class CreateTaskUseCase : IUseCase<CreateTaskInput, CreateTaskOutput> {

        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly ITaskItemRepository taskRepo;
        private readonly IClock clock;

        #region AddTaskUseCase Constructor

        public CreateTaskUseCase(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            ITaskItemRepository taskRepo,
            IClock clock) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.taskRepo = taskRepo ?? throw new ArgumentNullException(nameof(taskRepo));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        #endregion

        public CreateTaskInput Input { set; private get; } = null;

        public CreateTaskOutput Output { get; private set; } = null;

        public void Execute() {
            //validate input data

            //create new TaskItem from input data
            TaskItem newTask = new TaskItem(
                Input.Title,
                Input.Description,
                new task_scheduler_entities.Colour(
                    Input.R, Input.B, Input.G
                ),
                Input.StartTime,
                notificationManager,
                /*assuming custom frequency for now*/
                new Frequencies.ConstantFrequency(Input.CustomFrequency),
                clock
            );

            //add task to task manager, check for errors
            if (taskManager.Add(newTask)) {
                //create DA TaskItem from new TaskItem
                //add task to task repo, check for errors

                //fill out output data and return
                Output = new CreateTaskOutput() { Success = true };
            }
            else {
              //fill out output data and return
              Output = new CreateTaskOutput() { Success = false, Error = "ERROR" };
            }

        }
    }
}
