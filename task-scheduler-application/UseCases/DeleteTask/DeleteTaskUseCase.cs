﻿using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_data_access.DataObjects;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;
using task_scheduler_utility;

namespace task_scheduler_application.UseCases.DeleteTask {
    /// <summary>
    /// Delete a TaskItem from the Application
    /// </summary>
    public class DeleteTaskUseCase : IUseCase<DeleteTaskUseCaseInput, DeleteTaskUseCaseOutput> {

        /// <summary>
        /// For finding and deleting task items in db
        /// </summary>
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;
        /// <summary>
        /// For finding and deleting notifications generated by a task, in db
        /// </summary>
        private readonly INotificationRepositoryFactory notificationRepositoryFactory;
        /// <summary>
        /// For finding and deleting tasks in domain
        /// </summary>
        private readonly BasicTaskManager taskItemManager;
        /// <summary>
        /// for finding and deleting notifications generated by tasks in the domain
        /// </summary>
        private readonly BasicNotificationManager notificationManager;

        public DeleteTaskUseCase(
            ITaskItemRepositoryFactory taskItemRepositoryFactory,
            INotificationRepositoryFactory notificationRepositoryFactory,
            BasicTaskManager taskItemManager,
            BasicNotificationManager notificationManager) {

            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.notificationRepositoryFactory = notificationRepositoryFactory ?? throw new ArgumentNullException(nameof(notificationRepositoryFactory));
            this.taskItemManager = taskItemManager ?? throw new ArgumentNullException(nameof(taskItemManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
        }

        /// <summary>
        /// Deletes a Task Item from the application
        /// </summary>
        /// <param name="input">
        /// Holds the input necessary for deleting a TaskItem
        /// </param>
        /// <returns>
        /// The result of deleting the TaskItem
        /// </returns>
        public DeleteTaskUseCaseOutput Execute(DeleteTaskUseCaseInput input) {
            Guid idToDelete = input.Id;

            ITaskItemRepository taskRepo = taskItemRepositoryFactory.New();

            //retrieve task item to delete
            Maybe<TaskItemDAL> maybeTask = taskRepo.GetById(idToDelete);

            if (maybeTask.HasValue) {
                TaskItemDAL taskToDelete = maybeTask.Value;

                //delete task item from repository
                if (taskRepo.Delete(idToDelete) == false) {
                    //unable to delete task
                    return new DeleteTaskUseCaseOutput() { Error = "Unable to to delete TaskItem", Success = false };
                }

                //delete task item from domain
                if(taskItemManager.Remove(idToDelete) == false) {
                    //failed to remove TaskItem from domain
                    //TODO : handle this appropriately
                }

                INotificationRepository notificationRepo = notificationRepositoryFactory.New();

                //iterate through notifications in repository and delete those generated by the deleted task
                foreach(NotificationDAL notificationDAL in notificationRepo.GetAll()) {
                    if(notificationDAL.taskId == idToDelete) {
                        if(notificationRepo.Delete(notificationDAL) == false) {
                            //unable to delete notification
                            //TODO : handle this appropriately
                        }
                    }
                }

                //delete task notifications in domain
                if(notificationManager.Remove(idToDelete) == false) {
                    //failed to remove task notifications from domain
                    //TODO : handle this appropriately
                }

                notificationRepo.Save();
                taskRepo.Save();
            }
            else {
                return new DeleteTaskUseCaseOutput() { Error = "Unable to find TaskItem to delete", Success = false };
            }

            return new DeleteTaskUseCaseOutput() { Success = true };
        }
    }
}
