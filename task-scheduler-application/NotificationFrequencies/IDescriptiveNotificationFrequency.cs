using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;

namespace task_scheduler_application.NotificationFrequencies {
    /// <summary>
    /// Defines the interface for a class implementing INotificationFrequency that
    /// also stores a string description of itself
    /// </summary>
    public interface IDescriptiveNotificationFrequency : INotificationFrequency{
        /// <summary>
        /// Description of the NotificationFrequency
        /// </summary>
        string Description { get; }
    }
}
