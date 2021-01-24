using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.DTO;
using task_scheduler_application.NotificationFrequencies;
using task_scheduler_application.UseCases.CreateTask;
using task_scheduler_application.UseCases.ViewNotifications;
using task_scheduler_application.UseCases.ViewTasks;
using task_scheduler_data_access.DataObjects;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application {
    //maybe make public into non-static class
    public class TaskSchedulerApplication {

        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;
        private readonly INotificationFrequencyRepositoryFactory notificationFrequencyRepositoryFactory;
        private readonly INotificationRepositoryFactory notificationRepositoryFactory;
        private readonly INotificationManager notificationManager;
        private readonly ITaskManager taskManager;
        private readonly IClock clock;

        private readonly CreateTaskUseCaseFactory createTaskUseCaseFactory;
        private readonly ViewTasksUseCaseFactory viewTasksUseCaseFactory;
        private readonly ViewNotificationsUseCaseFactory viewNotificationsUseCaseFactory;

        public TaskSchedulerApplication(
            ITaskItemRepositoryFactory taskItemRepositoryFactory,
            INotificationFrequencyRepositoryFactory notificationFrequencyRepositoryFactory,
            INotificationRepositoryFactory notificationRepositoryFactory,
            INotificationManager notificationManager,
            ITaskManager taskManager,
            IClock clock) {

            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.notificationFrequencyRepositoryFactory = notificationFrequencyRepositoryFactory ?? throw new ArgumentNullException(nameof(notificationFrequencyRepositoryFactory));
            this.notificationRepositoryFactory = notificationRepositoryFactory ?? throw new ArgumentNullException(nameof(notificationRepositoryFactory));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));

            this.InitializeDomainFromDatabase(
                taskItemRepositoryFactory,
                notificationRepositoryFactory,
                notificationManager,
                taskManager,
                clock
            );

            //CREATE USE-CASE FACTORIES
            createTaskUseCaseFactory =
                new CreateTaskUseCaseFactory(
                    taskManager,
                    notificationManager,
                    clock,
                    taskItemRepositoryFactory
                );

            viewTasksUseCaseFactory =
                new ViewTasksUseCaseFactory(taskManager, taskItemRepositoryFactory);

            viewNotificationsUseCaseFactory =
                new ViewNotificationsUseCaseFactory(
                    taskItemRepositoryFactory,
                    notificationRepositoryFactory
                );
        }

        //private readonly CreateTaskUseCaseFactory(){
        //}

        public CreateTaskUseCase NewCreateTaskUseCase() {
            return createTaskUseCaseFactory.New();
        }
        public ViewTasksUseCase NewViewTasksUseCase() {
            return viewTasksUseCaseFactory.New();
        }

        public ViewNotificationsUseCase NewViewNotificationsUseCase() {
            return viewNotificationsUseCaseFactory.New();
        }

        //invoked when new Notifications are generated
        public event EventHandler<NotificationDTO> NotificationAdded;

        protected void OnNotificationAdded(object source, Notification notification) {

            //update database with new notification
            NotificationDAL notificationDal = new NotificationDAL {
                taskId = notification.Producer.ID,
                time = notification.Time
            };

            INotificationRepository notificationRepo = notificationRepositoryFactory.New();
            notificationRepo.Add(notificationDal);

            ITaskItemRepository taskRepo = taskItemRepositoryFactory.New();
            List<TaskItemDAL> tasks = new List<TaskItemDAL>(taskRepo.GetAll());

            if (notification.Producer is TaskItem task) {

                //update the notifications producer in the database
                TaskItemDAL taskItemDAL = tasks.Find(t => t.id == task.ID);
                taskItemDAL.lastNotificationTime = task.LastNotificationTime;
                if(taskRepo.Update(taskItemDAL) == false) {
                    //could not update task in database
                }

                //create DTO to invoke NotificationAdded with
                NotificationDTO dto = new NotificationDTO() {
                    TaskId = task.ID,
                    Time = notification.Time,
                    Title = notification.Producer.Title,
                    R = task.Colour.R,
                    G = task.Colour.G,
                    B = task.Colour.B
                };

                //invoked event delegates
                NotificationAdded?.Invoke(source, dto);
            }

            taskRepo.Save();
            notificationRepo.Save();
        }

        public void InitializeDomainFromDatabase(
            ITaskItemRepositoryFactory taskItemRepositoryFactory,
            INotificationRepositoryFactory notificationRepositoryFactory,
            INotificationManager notificationManager,
            ITaskManager taskManager,
            IClock clock) {

            /*
             * hook up to notfication added event now, so to retrieve new notifications generated by
             * the TaskItems being created below
             */
            notificationManager.NotificationAdded += OnNotificationAdded;

            /*
             * read in task items from database. Create domain taskItems from 
             * data and add items to taskManager
             */
            ITaskItemRepository taskItemRepository = taskItemRepositoryFactory.New();
            foreach (TaskItemDAL task in taskItemRepository.GetAll()) {

                INotificationFrequency notificationFrequency = null;

                if (task.customNotificationFrequency.HasValue) {
                    CustomNotificationFrequencyDAL frequencyDAL = task.customNotificationFrequency.Value;

                    notificationFrequency = NotificationFrequencyFactory.New(
                        //TODO: do something safer than just a cast
                        (NotificationFrequencyType)task.notificationFrequencyType,
                        frequencyDAL.time
                    );
                }
                else {
                    notificationFrequency = NotificationFrequencyFactory.New(
                        //TODO: do something safer than just a cast
                        (NotificationFrequencyType)task.notificationFrequencyType
                    );
                }

                taskManager.Add(
                    new TaskItem(
                        task.title,
                        task.description,
                        new Colour(task.r, task.g, task.b),
                        task.startTime,
                        notificationManager,
                        notificationFrequency,
                        clock,
                        task.lastNotificationTime,
                        task.id
                    )
                );
            }
        }
    }
}
