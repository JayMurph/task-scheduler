using System;
using System.Collections.Generic;
using System.Text;
using task_scheduler_entities;

namespace task_scheduler_application.NotificationFrequencies {

    /// <summary>
    /// Produces different concrete implementations of IDescriptiveNotificationFrequency's
    /// </summary>
    public static class NotificationFrequencyFactory {

        /// <summary>
        /// Produces an IDescriptiveNotificationFrequency corresponding to the value of the 'type'
        /// parameter
        /// </summary>
        /// <param name="type">
        /// String identifying which implementation of
        /// IDescriptiveNotificationFrequency to produce and return.
        /// </param>
        /// <param name="customPeriod">
        /// A custom time interval to initialize a CustomNotificationFrequency with, if the
        /// 'type' requested is "Custom"
        /// </param>
        /// <returns>
        /// An implementation of IDescriptiveNotificationFrequency corresponding to the 'type'
        /// parameter, and initialize with the 'customPeriod' parameter if the value of 'type' is
        /// "Custom". null is returned if the 'type' parameter value does not describe a pre-defined
        /// type
        /// </returns>
        public static INotificationFrequency New(NotificationFrequencyType type, TimeSpan customPeriod = new TimeSpan()) {
            //TODO: protect against invalid customPeriod
            switch (type) {
                case NotificationFrequencyType.Daily : return new DailyNotificationFrequency();
                case NotificationFrequencyType.AlternateDay : return new AlternateDayNotificationFrequency();
                case NotificationFrequencyType.Review : return new ReviewNotificationFrequency();
                case NotificationFrequencyType.Custom : return new CustomNotificationFrequency(customPeriod);
                default: return null;
            }
        }
    }
}
