﻿using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_entities {
    /// <summary>
    /// Can be scheduled to produce notifications at pre-determined intervals.
    /// </summary>
    public class TaskItem : ITaskItem{
        #region Fields 
        /// <summary>
        /// Used to determine times at which the TaskItem should produce a Notification
        /// </summary>
        private INotificationFrequency frequency;

        /// <summary>
        /// Receives the notifications generated by the TaskItem
        /// </summary>
        private INotificationManager manager;

        /// <summary>
        /// Tracks time for the TaskItem
        /// </summary>
        private IClock clock;

        /// <summary>
        /// Unique identifier for the TaskItem
        /// </summary>
        private readonly Guid id;

        /// <summary>
        /// Used for adding the Notification generated by the TaskItem to its 
        /// INotificationManager as an asynchronous operation after a set period of time
        /// </summary>
        private DelayedTask notifier;
        #endregion

        #region Properties
        /// <summary>
        /// The time at which the TaskItem is scheduled to begin producing notifications 
        /// </summary>
        private DateTime startTime;
        private bool disposedValue;

        /// <summary>
        /// Title of the TaskItem
        /// </summary>
        public string Title{get;set;}

        /// <summary>
        /// Extra descriptive information for the TaskItem
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Colour of the TaskItem when it is visually represented
        /// </summary>
        public Colour Colour { get; set; }

        /// <summary>
        /// The most recent time at which the TaskItem produced a notification. A value of
        /// DateTime.MinValue indicates that the TaskItem has not produced a any Notifications yet.
        /// </summary>
        public DateTime LastNotificationTime { get; private set; }

        /// <summary>
        /// The time at which the TaskItem is scheduled to being producing notifications
        /// </summary>
        public DateTime StartTime {
            get {
                return startTime;
            }
            set {
                startTime = value;
                if (IsActive) {
                    ScheduleNextNotification();
                }
            }
        }

        /// <summary>
        /// Unique identifier for the TaskItem
        /// </summary>
        public Guid ID { get => id; }

        /// <summary>
        /// Indicates if the TaskItem is set to produce notifications
        /// </summary>
        public bool IsActive { get; private set; } = false;

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new TaskItem, assigns the incoming parameters to the appropriate properties,
        /// and makes the TaskItem active
        /// </summary>
        /// <param name="title">Title text for the TaskItem</param>
        /// <param name="description">Extra descriptive info for the TaskItem</param>
        /// <param name="colour">Colour for visually representing the Notifications produced by the TaskItem</param>
        /// <param name="startTime">When the TaskItem should begin scheduling notifications</param>
        /// <param name="manager">Collects the notifications generated by the TaskItem</param>
        /// <param name="frequency">Determines the times at which the TaskItem should generate notifications</param>
        /// <param name="clock">Provides the current time for the TaskItem</param>
        /// <param name="lastNotificationTime">The most recent point in time at which the TaskItem
        /// produced a notification. Set to DateTime.MinValue to indicate that the TaskItem has
        /// never generated a notification
        /// </param>
        /// <param name="id">Unique identifier for a TaskItem</param>
        public TaskItem(
            string title,
            string description,
            Colour colour,
            DateTime startTime,
            INotificationManager manager,
            INotificationFrequency frequency,
            IClock clock,
            DateTime lastNotificationTime,
            Guid id
            ) {

            Title = title;
            Description = description;
            Colour = colour;
            LastNotificationTime = lastNotificationTime;

            this.id = id;

            this.startTime = startTime;

            this.manager = manager;
            this.frequency = frequency;
            this.clock = clock;

            while(IsOverdue()){
                //post over-due notifications until we catch up to the present
                PostNotification(frequency.NextNotificationTime(this.StartTime, this.LastNotificationTime)); 
            }

            IsActive = true;

            ScheduleNextNotification();
        }

        /// <summary>
        /// Creates a new TaskItem, assigns the incoming parameters to the appropriate properties,
        /// and makes the TaskItem active
        /// </summary>
        /// <param name="title">Title text for the TaskItem</param>
        /// <param name="description">Extra descriptive info for the TaskItem</param>
        /// <param name="colour">Colour for visually representing the Notifications produced by the TaskItem</param>
        /// <param name="startTime">When the TaskItem should begin scheduling notifications</param>
        /// <param name="manager">Collects the notifications generated by the TaskItem</param>
        /// <param name="frequency">Determines the times at which the TaskItem should generate notifications</param>
        /// <param name="clock">Provides the current time for the TaskItem</param>
        public TaskItem(
            string title,
            string description,
            Colour colour,
            DateTime startTime,
            INotificationManager manager,
            INotificationFrequency frequency,
            IClock clock
            ):
            this(
                title,
                description,
                colour,
                startTime,
                manager,
                frequency,
                clock,
                DateTime.MinValue,
                Guid.NewGuid()
                ) {
        }
        #endregion

        /// <summary>
        /// Assigns an INotificationFrequency to the TaskItem, for it to use to determine
        /// the points in time at which to generate notifications
        /// </summary>
        /// <param name="frequency">
        /// To be assigned to the TaskItem and used for determining when the TaskItem should
        /// produce notifications
        /// </param>
        ///<exception cref="InvalidOperationException">TaskItem is no longer active</exception>
        public void ChangeFrequency(INotificationFrequency frequency) {
            if (IsActive) {
                this.frequency = frequency ?? throw new ArgumentNullException(nameof(frequency));

                //cancel our currently scheduled notification
                notifier?.Cancel();

                while(IsOverdue()){
                    //post any notifications that we missed according to the new frequency
                    PostNotification(frequency.NextNotificationTime(this.StartTime, this.LastNotificationTime)); 
                }

                ScheduleNextNotification();
            }
            else {
                throw new InvalidOperationException($"{nameof(TaskItem)} is not longer active");
            }
        }

        /// <summary>
        /// Stops the TaskItem from producing Notifications, making it inactive. The TaskItem
        /// cannot be restarted once this method is called.
        /// </summary>
        public void Cancel() {
            notifier.Cancel();
            IsActive = false;
        }

        /// <summary>
        /// Disposes of the TaskItem's resources and makes it Inactive
        /// </summary>
        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects)
                    //if the TaskItem is active, then cancel it
                    if (IsActive) {
                        Cancel();
                    }
                    notifier.Dispose();
                    notifier = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TaskItem()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        /// <summary>
        /// Indicates if the TaskItem is overdue for producing a Notification.
        /// </summary>
        /// <returns>
        /// True if the TaskItem should have produced a Notification according to its period and
        /// clock.
        /// </returns>
        private bool IsOverdue() {
            return frequency.NextNotificationTime(this.StartTime, this.LastNotificationTime) < clock.Now;
        }

        /// <summary>
        /// Schedules a new DelayedTask for the TaskItem, that will send a new Notification to the
        /// TaskItem's INotificationManager at the time determined by it's INotificationPeriod
        /// </summary>
        private void ScheduleNextNotification() {
            //calculate the next time to post a notification
            DateTime nextNotificationTime = frequency.NextNotificationTime(StartTime, clock.Now);

            //schedule posting the next notification at the appropriate time
            notifier = 
                new DelayedTask(
                    () => { PostNotification(clock.Now); ScheduleNextNotification(); }, 
                    nextNotificationTime, 
                    clock
                );
        }

        /// <summary>
        /// Creates a Notification from the TaskItem and the incoming timeOfNotification,
        /// and adds it to the collection of the TaskItem's INotificationManager
        /// </summary>
        /// <param name="timeOfNotification">
        /// Time-stamp for the new Notification
        /// </param>
        private void PostNotification(DateTime timeOfNotification) {
            //create new notification and add it to the NotificationManager
            Notification notification = new Notification(Guid.NewGuid(), this, timeOfNotification);
            LastNotificationTime = timeOfNotification;
            manager.Add(notification);
        }

    }
}
