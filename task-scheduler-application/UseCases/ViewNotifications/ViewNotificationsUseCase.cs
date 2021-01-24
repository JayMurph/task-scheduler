using System;
using System.Collections.Generic;
using task_scheduler_application.DTO;
using task_scheduler_data_access.DataObjects;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;
using task_scheduler_utility;

namespace task_scheduler_application.UseCases.ViewNotifications {
    public class ViewNotificationsUseCase : IUseCase<ViewNotificationsInput, ViewNotificationsOutput> {

        //used for retrieving notifications 
        private readonly INotificationRepositoryFactory notificationRepositoryFactory;

        //used for retrieving data of task associated to a notification
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewNotificationsUseCase(
            INotificationRepositoryFactory notificationRepositoryFactory, ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.notificationRepositoryFactory = notificationRepositoryFactory ?? throw new ArgumentNullException(nameof(notificationRepositoryFactory));
        }

        /// <summary>
        /// Executes the ViewNotificationsUseCase. Sets the Output property of the UseCase object
        /// once complete
        /// </summary>
        public ViewNotificationsOutput Execute(ViewNotificationsInput input) {
            //no input for use case 
            ITaskItemRepository taskRepo = taskItemRepositoryFactory.New();

            ViewNotificationsOutput output = new ViewNotificationsOutput {
                Success = true
            };

            /*
             * Get all notifications that are present in the application, convert them to DTO's, then
             * add them to a collection to return
             */
            INotificationRepository notificationRepository = notificationRepositoryFactory.New();
            foreach(NotificationDAL notification in notificationRepository.GetAll()) {

                Maybe<TaskItemDAL> maybeTask = taskRepo.GetById(notification.taskId);

                if(maybeTask.HasValue) {
                    TaskItemDAL taskDAL = maybeTask.Value;
                    NotificationDTO dto = new NotificationDTO() {
                        TaskId = taskDAL.id,
                        Title = taskDAL.title,
                        Time = notification.time,
                        R = taskDAL.r,
                        G = taskDAL.g, 
                        B = taskDAL.b
                    };

                    output.Notifications.Add(dto);
                }

            }

            return output;
        }
    }
}
