using System;

namespace task_scheduler_entities {
    /// <summary>
    /// Interface for classes that track time and allow retrieval of the current time.
    /// </summary>
    public interface IClock {
        /// <summary>
        /// The current time
        /// </summary>
        DateTime Now { get; }
    }
}
