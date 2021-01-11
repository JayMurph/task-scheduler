﻿using System;
using System.Threading.Tasks;
using System.Threading;
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

            asyncTask = Task.Factory.StartNew(
                () => {
                    //wait until dueTime is reached or stopSignal is activated
                    SpinWait.SpinUntil(() => { return (clock.Now >= dueTime || stopSignal); });

                    //perform the given action if we did not receive the stop signal
                    if (!stopSignal) { action(); }
                }, 
                TaskCreationOptions.LongRunning
            );
        }

        /// <summary>
        /// Stops the action assigned to the DelayedTask from executing. The DelayedTask
        /// is no longer usable for performing the action after this method is called.
        /// </summary>
        public virtual void Cancel() {
            if (!stopSignal) {
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
        }
    }
}
