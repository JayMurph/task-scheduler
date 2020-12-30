using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;

namespace task_scheduler_application.UseCases.AddTask {
    class AddTaskUseCaseFactory : IUseCaseFactory<AddTaskUseCase> {

        ITaskManager taskManager;
        INotificationManager notificationManager;
        //task repo
        IClock clock;

        public AddTaskUseCase New() {
            throw new NotImplementedException();
            //return a new AddTaskUseCase
        }
    }
}
