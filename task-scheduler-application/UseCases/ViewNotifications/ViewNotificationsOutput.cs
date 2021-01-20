using System.Collections.Generic;
using task_scheduler_application.DTO;

namespace task_scheduler_application.UseCases.ViewNotifications {
    public class ViewNotificationsOutput : UseCaseOutput {
        public List<NotificationDTO> Notifications { get; set; } = new List<NotificationDTO>();
    }
}
