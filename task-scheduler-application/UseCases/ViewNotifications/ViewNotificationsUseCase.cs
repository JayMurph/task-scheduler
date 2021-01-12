using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.UseCases.ViewNotifications {
    public class ViewNotificationsUseCase : IUseCase<ViewNotificationsInput, ViewNotificationsOutput> {
        public ViewNotificationsInput Input { set; private get; }

        public ViewNotificationsOutput Output { get; private set; }

        public void Execute() {
            //no input for use case 



        }
    }
}
