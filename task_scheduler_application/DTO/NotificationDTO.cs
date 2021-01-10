using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.DTO {
    public class NotificationDTO {
        Guid Id { get; set; }
        TaskItemDTO TaskItemDTO { get; set; }
        DateTime Time { get; set; }
    }
}
