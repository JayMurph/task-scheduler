﻿using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_application.Frequencies;
using task_scheduler_data_access_standard.Repositories;

namespace task_scheduler_application.UseCases.CreateTask {
    public class CreateTaskUseCase : IUseCase<CreateTaskInput, CreateTaskOutput> {

        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly IClock clock;
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        #region AddTaskUseCase Constructor

        public CreateTaskUseCase(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            IClock clock, 
            ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
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

                //create DA TaskItem and frequency from new TaskItem

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
