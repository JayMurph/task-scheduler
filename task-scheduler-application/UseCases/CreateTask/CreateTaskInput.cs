using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;
using task_scheduler_application.NotificationFrequencies;

namespace task_scheduler_application.UseCases.CreateTask {
    /// <summary>
    /// Encapsulates the input required for a <see cref="CreateTaskUseCase"/>
    /// </summary>
    public class CreateTaskInput {

        /// <summary>
        /// Title to give to a new TaskItem
        /// </summary>
        public string Title;

        /// <summary>
        /// Description to give to a new TaskItem
        /// </summary>
        public string Description;

        /// <summary>
        /// The time at which the new TaskItem will become active, and thenceforth will begin
        /// producing Notifications each time its NotificationFrequency elapses.
        /// </summary>
        public DateTime StartTime;

        /// <summary>
        /// R value of the RGB colour assigned to the TaskItem
        /// </summary>
        public byte R;

        /// <summary>
        /// G value of the RGB colour assigned to the TaskItem
        /// </summary>
        public byte G;

        /// <summary>
        /// B value of the RGB colour assigned to the TaskItem
        /// </summary>
        public byte B;

        public NotificationFrequencyType NotificationFrequencyType;

        /// <summary>
        /// A unique NotificationFrequency to dictate when the new TaskItem should produce
        /// notifications
        /// </summary>
        public TimeSpan CustomNotificationFrequency;
    }
}
