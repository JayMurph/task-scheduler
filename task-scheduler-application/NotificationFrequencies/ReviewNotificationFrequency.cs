using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application.NotificationFrequencies {
    /// <summary>
    /// Implements a NotificationFrequency that activates at increasingly larger intervals (the
    /// interval doubles). The frequency is meant to emulate the ideal amount of time to wait
    /// between reviewing newly learnt information.
    /// </summary>
    class ReviewNotificationFrequency : IDescriptiveNotificationFrequency {
        /// <summary>
        /// Amount of time before first Notification. 
        /// </summary>
        private readonly TimeSpan startingPeriod = new TimeSpan(12, 0, 0);

        //TODO : abstract away magic string
        public string Description { get => "Review"; }


        /// <summary>
        /// Produces the next point in time at which a Notification should be produced by a TaskItem, according to
        /// the NotificationFrequency
        /// </summary>
        /// <param name="taskStartTime">The point in time at which a TaskItem became active</param>
        /// <param name="now">The current time</param>
        /// <returns>An upcoming point in time at which a TaskItem should produce a Notification</returns>
        public DateTime NextNotificationTime(DateTime taskStartTime, DateTime now) {
            TimeSpan periodAccum = startingPeriod;

            while(taskStartTime + periodAccum <= now) {
                //periodAccum = periodAccum * 2
                periodAccum = periodAccum.Add(periodAccum);
            }

            return taskStartTime + periodAccum;
        }

        /// <summary>
        /// Produces the length of time remaining until a TaskItem should produce its next
        /// Notification, according to the NotificationFrequency
        /// </summary>
        /// <param name="taskStartTime">The point in time at which a TaskItem became active</param>
        /// <param name="now">The current time</param>
        /// <returns>The time remaining until a TaskItem should produce its next Notification</returns>
        public TimeSpan TimeUntilNextNotification(DateTime taskStartTime, DateTime now) {
            return NotificationFrequencyUtility.TimeUntilNextNotification(taskStartTime, now, NextNotificationTime);
        }
    }
}
