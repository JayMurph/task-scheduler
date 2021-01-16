using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.CreateTask {
    /// <summary>
    /// Produces <see cref="CreateTaskUseCase"/>s that are initialized with their required
    /// dependencies
    /// </summary>
    public class CreateTaskUseCaseFactory : IUseCaseFactory<CreateTaskUseCase> {


        private readonly ITaskManager taskManager;
        private readonly INotificationManager notificationManager;
        private readonly IClock clock;
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        /// <summary>
        /// Constructs a new <see cref="CreateTaskUseCaseFactory"/>
        /// </summary>
        /// <param name="taskManager">
        /// </param>
        /// <param name="notificationManager">
        /// </param>
        /// <param name="clock">
        /// </param>
        /// <param name="taskItemRepositoryFactory">
        /// </param>
        /// <exception cref="ArgumentNullException">One of the arguments provided was null</exception>
        public CreateTaskUseCaseFactory(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            IClock clock,
            ITaskItemRepositoryFactory taskItemRepositoryFactory
            ){
            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        /// <summary>
        /// Returns a new <see cref="CreateTaskUseCase"/> object
        /// </summary>
        /// <returns></returns>
        public CreateTaskUseCase New() {
            return new CreateTaskUseCase(
                taskManager,
                notificationManager,
                clock,
                taskItemRepositoryFactory
            );
        }
    }
}
