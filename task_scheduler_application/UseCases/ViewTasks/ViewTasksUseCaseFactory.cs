using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_data_access_standard.Repositories;

namespace task_scheduler_application.UseCases.ViewTasks {
    public class ViewTasksUseCaseFactory : IUseCaseFactory<ViewTasksUseCase> {
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;
        private readonly INotificationFrequencyRepositoryFactory frequencyRepositoryFactory;

        public ViewTasksUseCaseFactory(ITaskItemRepositoryFactory taskItemRepositoryFactory, INotificationFrequencyRepositoryFactory frequencyRepositoryFactory) {
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
            this.frequencyRepositoryFactory = frequencyRepositoryFactory ?? throw new ArgumentNullException(nameof(frequencyRepositoryFactory));
        }

        public ViewTasksUseCase New() {
            return new ViewTasksUseCase(taskItemRepositoryFactory, frequencyRepositoryFactory);
        }
    }
}
