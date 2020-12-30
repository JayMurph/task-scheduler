using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_entities {
    /// <summary>
    /// The product of an ITaskItem when its notification time arrives
    /// </summary>
    public class Notification {

        /// <summary>
        /// Unique identifier for the Notification
        /// </summary>
        private readonly Guid id;

        /// <summary>
        /// Unique identifier for the Notification
        /// </summary>
        public Guid ID { get => id; }

        /// <summary>
        /// The TaskItem that produced the Notification
        /// </summary>
        public ITaskItem Producer { get; private set; }

        /// <summary>
        /// The time at which the Notification was produced
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Creates a new Notification. Assigns the incoming parameter's to the appropriate
        /// properties
        /// </summary>
        /// <param name="id">Unique identifier for the Notification</param>
        /// <param name="producer">
        /// The TaskItem that is generating the Notification
        /// </param>
        /// <param name="timeOfNotification">
        /// The time at which the notification is being generated
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The producer parameter was null
        /// </exception>
        public Notification(Guid id, ITaskItem producer, DateTime timeOfNotification) {
            this.id = id;
            Producer = producer ?? throw new ArgumentNullException(nameof(producer));
            Time = timeOfNotification;
        }
        
        /// <summary>
        /// Creates a new Notification. Assigns the incoming parameter's to the appropriate
        /// properties
        /// </summary>
        /// <param name="producer">
        /// The TaskItem that is generating the Notification
        /// </param>
        /// <param name="timeOfNotification">
        /// The time at which the notification is being generated
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The producer parameter was null
        /// </exception>
        public Notification(ITaskItem producer, DateTime timeOfNotification) 
            :this(Guid.NewGuid(), producer, timeOfNotification){
        }
    }
}
