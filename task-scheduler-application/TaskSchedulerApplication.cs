﻿using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.NotificationFrequencies;
using task_scheduler_data_access.DataObjects;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application {
    public static class TaskSchedulerApplication {

        /// <summary>
        /// Creates and adds neccessary domain objects to the domain manager classes
        /// </summary>
        /// <param name="taskItemRepositoryFactory"></param>
        /// <param name="notificationRepositoryFactory"></param>
        /// <param name="notificationManager"></param>
        /// <param name="taskManager"></param>
        /// <param name="clock"></param>
        public static void InitializeDomainFromDatabase(
            TaskItemRepositoryFactory taskItemRepositoryFactory,
            NotificationRepositoryFactory notificationRepositoryFactory,
            BasicNotificationManager notificationManager,
            BasicTaskManager taskManager,
            RealTimeClock clock) {

            //load database data into domain managers
            ITaskItemRepository taskItemRepository = taskItemRepositoryFactory.New();

            //read in task items from database. Create domain taskItems from 
            //data and add items to taskManager
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

            INotificationRepository notificationRepo = notificationRepositoryFactory.New();

            /*
             * read in notifications from database, create domain Notifications from data and store
             * them in the NotificationManager
             */
            foreach(NotificationDAL notification in notificationRepo.GetAll()) {
                ITaskItem producer = taskManager.Find(notification.taskId);

                if(producer == null) {
                    continue;
                }

                notificationManager.Add(
                    new Notification(producer, notification.time)
                );
            }
        }
    }
}
