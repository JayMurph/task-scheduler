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

        /// <summary>
        /// Indiciates if the Elements in CreateTaskInput are valid
        /// </summary>
        /// <param name="input">
        /// Will have its fields validated
        /// </param>
        /// <returns>
        /// True if all the fields of the input parameter are valid, otherwise false
        /// </returns>
        public static bool IsValid(CreateTaskInput input) {
            //Ensure Title is not null or empty
            if (string.IsNullOrWhiteSpace(input.Title)) {
                return false;
            }

            /*
             * Ensure that if the Input has a Frequency type of Custom, that its
             * CustomNotificationFrequency is valid
             */
            if(input.NotificationFrequencyType == NotificationFrequencyType.Custom) { 
                if(input.CustomNotificationFrequency == null) {
                    return false;
                }
                else if(input.CustomNotificationFrequency == TimeSpan.Zero) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a string describing errors present in the input parameter
        /// </summary>
        /// <param name="input">
        /// To be examined for errors
        /// </param>
        /// <returns>
        /// Describes any errors present in the input parameter
        /// </returns>
        public static string MakeErrorMessage(CreateTaskInput input) {
            StringBuilder errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(input.Title)) {
                errorBuilder.AppendLine("Task title cannot be empty.");
            }
            
            if(input.NotificationFrequencyType == NotificationFrequencyType.Custom &&
                input.CustomNotificationFrequency == TimeSpan.Zero) {
                errorBuilder.AppendLine("A Task with a Custom Notification Frequency must have a non-zero TimeSpan.");
            }

            return errorBuilder.ToString();
        }
    }
}
