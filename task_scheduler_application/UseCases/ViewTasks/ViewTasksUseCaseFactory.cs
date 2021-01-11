﻿using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access_standard.Repositories;

namespace task_scheduler_application.UseCases.ViewTasks {
    public class ViewTasksUseCaseFactory : IUseCaseFactory<ViewTasksUseCase> {
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public ViewTasksUseCaseFactory(ITaskItemRepositoryFactory taskItemRepositoryFactory) {
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        public ViewTasksUseCase New() {
            return new ViewTasksUseCase(taskItemRepositoryFactory);
        }
    }
}
