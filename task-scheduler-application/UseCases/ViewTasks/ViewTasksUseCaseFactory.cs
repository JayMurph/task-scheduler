using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access_standard.Repositories;

namespace task_scheduler_application.UseCases.ViewTasks {
    /// <summary>
    /// Produces <see cref="ViewTasksUseCase"/>s that are initialized with their required
    /// dependencies
    /// </summary>
    public class ViewTasksUseCaseFactory : IUseCaseFactory<ViewTasksUseCase> {
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        /// <summary>
        /// Constructs a new ViewTasksUseCaseFactory
        /// </summary>
        /// <param name="taskItemRepositoryFactory">
        /// Dependency required for creating <see cref="ViewTasksUseCase"/>s. Produces <see cref="TaskItemRepository"/>s.
        /// </param>
        public ViewTasksUseCaseFactory(ITaskItemRepositoryFactory taskItemRepositoryFactory) {
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        /// <summary>
        /// Returns a new ViewTasksUseCase object
        /// </summary>
        /// <returns></returns>
        public ViewTasksUseCase New() {
            return new ViewTasksUseCase(taskItemRepositoryFactory);
        }
    }
}
