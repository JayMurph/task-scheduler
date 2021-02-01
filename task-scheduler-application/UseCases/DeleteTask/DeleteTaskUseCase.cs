﻿using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_data_access.DataObjects;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;
using task_scheduler_utility;

namespace task_scheduler_application.UseCases.DeleteTask {
    public class DeleteTaskUseCase : IUseCase<DeleteTaskUseCaseInput, DeleteTaskUseCaseOutput> {

        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;
        private readonly INotificationRepositoryFactory notificationRepositoryFactory;
        private readonly ITaskManager taskItemManager;
        private readonly INotificationManager notificationManager;

        public DeleteTaskUseCase(
            ITaskItemRepositoryFactory taskItemRepositoryFactory,
            INotificationRepositoryFactory notificationRepositoryFactory,
            ITaskManager taskItemManager,
            INotificationManager notificationManager) {

            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.notificationRepositoryFactory = notificationRepositoryFactory ?? throw new ArgumentNullException(nameof(notificationRepositoryFactory));
            this.taskItemManager = taskItemManager ?? throw new ArgumentNullException(nameof(taskItemManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
        }

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
                //TODO: give ITaskManager interface Remove(id) method
                //taskItemManager.Remove(idToDelete)

                INotificationRepository notificationRepo = notificationRepositoryFactory.New();

                //iterate through notifications in repository and delete those generated by the deleted task
                foreach(NotificationDAL notificationDAL in notificationRepo.GetAll()) {
                    if(notificationDAL.taskId == idToDelete) {
                        if(notificationRepo.Delete(notificationDAL) == false) {
                            //unable to delete notification
                        }
                    }
                }

                //delete task notifications in domain
                //TODO: give INotificationManager interface Remove(id) method
                //notificationManager.remove(idToDelete)


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