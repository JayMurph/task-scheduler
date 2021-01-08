using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_application.Frequencies;
using task_scheduler_data_access_standard.Repositories;
using task_scheduler_data_access_standard.DataObjects;

namespace task_scheduler_application.UseCases.CreateTask {
    public class CreateTaskUseCase : IUseCase<CreateTaskInput, CreateTaskOutput> {

        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly IClock clock;
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;
        private readonly IFrequencyRepositoryFactory frequencyRepositoryFactory;

        #region AddTaskUseCase Constructor

        public CreateTaskUseCase(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            IClock clock,
            ITaskItemRepositoryFactory taskItemRepositoryFactory, 
            IFrequencyRepositoryFactory frequencyRepositoryFactory) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.frequencyRepositoryFactory = frequencyRepositoryFactory ?? throw new ArgumentNullException(nameof(frequencyRepositoryFactory));
        }

        #endregion

        public CreateTaskInput Input { set; private get; } = null;

        public CreateTaskOutput Output { get; private set; } = null;

        public void Execute() {
            //validate input data

            IDescriptiveNotificationFrequency frequency = null;

            //create appropriate frequency for new TaskItem
            //TODO : abstract away magic string
            if(Input.FrequencyType == "Custom") {
                frequency = NotificationFrequencyFactory.New(Input.FrequencyType, Input.CustomFrequency);
            }
            else {
                frequency = NotificationFrequencyFactory.New(Input.FrequencyType);
            }

            //create new TaskItem from input data
            TaskItem newTask = new TaskItem(
                Input.Title,
                Input.Description,
                new task_scheduler_entities.Colour(
                    Input.R, Input.G, Input.B
                ),
                Input.StartTime,
                notificationManager,
                frequency,
                clock
            );

            //add task to task manager, check for errors
            if (taskManager.Add(newTask)) {
                //create DA TaskItem and frequency from new TaskItem
                ITaskItemRepository taskItemRepository = taskItemRepositoryFactory.New();

                //add task to task repo, check for errors
                taskItemRepository.Add(
                    new TaskItemDAL(
                        newTask.ID,
                        newTask.Title,
                        newTask.Description,
                        newTask.StartTime,
                        newTask.LastNotificationTime,
                        newTask.Colour.R,
                        newTask.Colour.G,
                        newTask.Colour.B,
                        frequency.Description
                    )
                );

                IFrequencyRepository frequencyRepository = null; 

                //add task frequency to database if it is a custom frequency
                //TODO : abstract away magic string
                if(frequency.Description == "Custom") {
                    frequencyRepository = frequencyRepositoryFactory.New();

                    // save custom frequency data to database
                    NotificationFrequencyDAL newFrequencyDAL = new NotificationFrequencyDAL(
                        newTask.ID,
                        Input.CustomFrequency
                    );

                    frequencyRepository.Add(newFrequencyDAL);
                }

                taskItemRepository.Save();

                //save frequency database if we created to save a custom frequency
                if(frequencyRepository != null) {
                    frequencyRepository.Save();
                }

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
