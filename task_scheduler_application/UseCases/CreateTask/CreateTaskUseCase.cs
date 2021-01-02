using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_application.Frequencies;
using task_scheduler_data_access;

namespace task_scheduler_application.UseCases.CreateTask {
    public class CreateTaskUseCase : IUseCase<CreateTaskInput, CreateTaskOutput> {

        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly IClock clock;

        #region AddTaskUseCase Constructor

        public CreateTaskUseCase(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            IClock clock) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
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
                    Input.R, Input.G, Input.B
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
                TaskItemUnitOfWork work = new TaskItemUnitOfWork();

                NotificationFrequencyDAL freq = new NotificationFrequencyDAL() {
                    NotificationFrequencyDALId = newTask.ID,
                    Frequency = Input.CustomFrequency
                };

                work.Repository.Insert(new TaskItemDAL() {
                    TaskItemDALId = newTask.ID,
                    Title = newTask.Title,
                    Description = newTask.Description,
                    R = newTask.Colour.R,
                    G = newTask.Colour.G,
                    B = newTask.Colour.B,
                    StartTime = newTask.StartTime,
                    /*using custome frequency for now*/
                    FrequencyType = "Custom",
                    NotificationFrequency = freq
                });

                //add task to task repo, check for errors
                work.Save();

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
