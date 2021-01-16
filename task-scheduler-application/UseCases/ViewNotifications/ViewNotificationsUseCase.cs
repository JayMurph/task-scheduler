using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application;
using task_scheduler_data_access.Repositories;
using task_scheduler_data_access.DataObjects;
using task_scheduler_application.DTO;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.ViewNotifications {
    public class ViewNotificationsUseCase : IUseCase<ViewNotificationsInput, ViewNotificationsOutput> {
        INotificationManager notificationManager;
        ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewNotificationsUseCase(
            INotificationManager notificationManager,
            ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        public ViewNotificationsInput Input { set; private get; } = null;

        public ViewNotificationsOutput Output { get; private set; } = null;

        public void Execute() {
            //no input for use case 
            ITaskItemRepository taskRepo = taskItemRepositoryFactory.New();

            List<NotificationDTO> notifications = new List<NotificationDTO>();

            /*
             * Get all notifications that are present in the application, convert them to DTO's, then
             * add them to a collection to return
             */
            foreach(Notification notification in notificationManager.GetAll()) {

                TaskItemDAL dataLayerTask = taskRepo.GetById(notification.Producer.ID);

                if(dataLayerTask == null) {
                    continue;
                }

                NotificationDTO dto = new NotificationDTO() {
                    TaskId = dataLayerTask.Id,
                    Title = dataLayerTask.Title,
                    Time = notification.Time,
                    R = dataLayerTask.R,
                    G = dataLayerTask.G, 
                    B = dataLayerTask.B
                };

                notifications.Add(dto);
            }

            Output = new ViewNotificationsOutput() { Success = true, Notifications = notifications };
        }
    }
}
