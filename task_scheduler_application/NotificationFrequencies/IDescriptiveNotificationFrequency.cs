using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;

namespace task_scheduler_application.NotificationFrequencies {
    public interface IDescriptiveNotificationFrequency : INotificationFrequency{
        string Description { get; }
    }
}
