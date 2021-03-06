﻿using System;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.ViewTasks {
    /// <summary>
    /// Produces <see cref="ViewTasksUseCase"/>s that are initialized with their required
    /// dependencies
    /// </summary>
    public class ViewTasksUseCaseFactory : IUseCaseFactory<ViewTasksUseCase> {
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;
        private readonly BasicTaskManager taskManager;

        public ViewTasksUseCaseFactory(BasicTaskManager taskManager, ITaskItemRepositoryFactory taskItemRepositoryFactory) {
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.taskManager = taskManager;
        }

        /// <summary>
        /// Returns a new ViewTasksUseCase object
        /// </summary>
        /// <returns></returns>
        public ViewTasksUseCase New() {
            return new ViewTasksUseCase(taskManager, taskItemRepositoryFactory);
        }
    }
}
