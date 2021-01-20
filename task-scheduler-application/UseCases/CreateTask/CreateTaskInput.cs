using System;
using System.Text;
using task_scheduler_application.NotificationFrequencies;

namespace task_scheduler_application.UseCases.CreateTask {
    /// <summary>
    /// Encapsulates the input required for a <see cref="CreateTaskUseCase"/>
    /// </summary>
    public class CreateTaskInput {

        /// <summary>
        /// Title to give to a new TaskItem
        /// </summary>
        public string Title {get;set;}

        /// <summary>
        /// Description to give to a new TaskItem
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The time at which the new TaskItem will become active, and thenceforth will begin
        /// producing Notifications each time its NotificationFrequency elapses.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// R value of the RGB colour assigned to the TaskItem
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// G value of the RGB colour assigned to the TaskItem
        /// </summary>
        public byte G{ get; set; }

        /// <summary>
        /// B value of the RGB colour assigned to the TaskItem
        /// </summary>
        public byte B{ get; set; }

        public NotificationFrequencyType NotificationFrequencyType { get; set; }

        /// <summary>
        /// A unique NotificationFrequency to dictate when the new TaskItem should produce
        /// notifications
        /// </summary>
        public TimeSpan CustomNotificationFrequency { get; set; }

        /// <summary>
        /// Indiciates if the Elements in CreateTaskInput are valid
        /// </summary>
        /// <param name="input">
        /// Will have its fields validated
        /// </param>
        /// <returns>
        /// True if all the fields of the input parameter are valid, otherwise false
        /// </returns>
        public bool IsValid() {
            //Ensure Title is not null or empty
            if (string.IsNullOrWhiteSpace(Title)) {
                return false;
            }

            /*
             * Ensure that if the Input has a Frequency type of Custom, that its
             * CustomNotificationFrequency is valid
             */
            if(NotificationFrequencyType == NotificationFrequencyType.Custom) { 
                if(CustomNotificationFrequency == null) {
                    return false;
                }
                else if(CustomNotificationFrequency == TimeSpan.Zero) {
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
        public string GetErrorMessage() {
            StringBuilder errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Title)) {
                errorBuilder.AppendLine("Task title cannot be empty.");
            }
            
            if(NotificationFrequencyType == NotificationFrequencyType.Custom &&
                CustomNotificationFrequency == TimeSpan.Zero) {
                errorBuilder.AppendLine("A Task with a Custom Notification Frequency must have a non-zero TimeSpan.");
            }

            return errorBuilder.ToString();
        }
    }
}
