using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.DTO {
    public struct NotificationDTO {
        Guid Id { get; set; }
        DateTime Time { get; set; }
    }
}
