using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_entities {
    /// <summary>
    /// Performs an asynchronous operation at a pre-determined point in time.
    /// </summary>
    public class DelayedTask {
        /// <summary>
        /// The time at which the DelayedTask should execute its assigned action
        /// </summary>
        private readonly DateTime dueTime;

        /// <summary>
        /// The time at which the DelayedTask should execute its assigned action
        /// </summary>
        public DateTime DueTime{ get { return dueTime; } }

        /// <summary>
        /// Allows the DelayedTask to retrieve the current time
        /// </summary>
        private readonly IClock clock;

        /// <summary>
        /// The asynchronous operation (waiting, then action) of the DelayedTask
        /// </summary>
        private readonly Task asyncTask;

        /// <summary>
        /// Flag for stopping the DelayedTasks assigned action from executing
        /// </summary>
        private volatile bool stopSignal = false;


        /// <summary>
        /// Creates a new DelayedTask object. Schedules the provided action to be performed
        /// after the dueTime is reached.
        /// </summary>
        /// <param name="action">
        /// Operation to be performed once dueTime occurs
        /// </param>
        /// <param name="dueTime">
        /// When to perform the action
        /// </param>
        /// <param name="clock">
        /// Provides the current time for the DelayedTask
        /// </param>
        public DelayedTask(Action action, DateTime dueTime, IClock clock) {
            this.dueTime = dueTime;
            this.clock = clock;

            asyncTask = Task.Factory.StartNew(() => DelayedOperation(action));
        }

        /// <summary>
        /// Stops the action assigned to the DelayedTask from executing. The DelayedTask
        /// is no longer usable for performing the action after this method is called.
        /// </summary>
        public virtual void Cancel() {
            stopSignal = true;

            try {
                asyncTask.Wait();
            }
            catch (Exception ex){
            }
            finally {
                asyncTask.Dispose();
            }
        }

        /// <summary>
        /// Performs the action provided once the point in time set as the DelayedTask's
        /// dueTime is reached. Can be exited early by calling the DelayedTask's Cancel method
        /// </summary>
        /// <param name="action">
        /// A method to call after the DelayedTask's dueTime is reached.
        /// </param>
        protected virtual void DelayedOperation(Action action) {
            while(clock.Now < dueTime) {
                if (stopSignal) {
                    return;
                }
            }

            action();
        }

    }
}
