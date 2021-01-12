using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_application.DTO;

namespace task_scheduler_application.UseCases.ViewNotifications {
    public class ViewNotificationsOutput : UseCaseOutput {
        public List<NotificationDTO> Notifications { get; set; } = new List<NotificationDTO>();
    }
}
