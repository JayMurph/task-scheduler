﻿using System;
using System.Collections.Generic;

namespace task_scheduler_entities {

    /// <summary>
    /// Interface for classes that collect notifications generated by TaskItems
    /// </summary>
    public interface INotificationManager {
        /// <summary>
        /// Event that occurs whenever a Notification is added to the 
        /// INotificationManager's collection
        /// </summary>
        event EventHandler<Notification> NotificationAdded;

        /// <summary>
        /// Removes the provided notification from the INotificationManager's collection
        /// </summary>
        /// <param name="notification">
        /// To be removed from the INotificationManager's collection
        /// </param>
        /// <returns>
        /// True if the Notification provided was removed from the INotificationManager's
        /// collection, otherwise false
        /// </returns>
        bool Remove(Notification notification);

        /// <summary>
        /// Adds a Notification to the INotificationManager's collection
        /// </summary>
        /// <param name="notification">
        /// To be added to the INotificationManager's collection
        /// </param>
        void Add(Notification notification);

        /// <summary>
        /// Retrieves a List containing all the Notifications in the INotificationManager's
        /// collection
        /// </summary>
        /// <returns>
        /// Contains the Notifications that are present in the INotificationManager's collection
        /// </returns>
        List<Notification> GetAll();
    }
}
