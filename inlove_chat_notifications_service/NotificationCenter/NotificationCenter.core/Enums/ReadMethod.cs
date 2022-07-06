namespace NotificationCenter.Core.Enums
{
    /// <summary>
    /// Indicates how a notification is marked as read.
    /// </summary>
    public enum ReadMethod
    {
        /// <summary>
        /// The notification has not been read.
        /// </summary>
        None,

        /// <summary>
        /// The user received a notification, tapped on it, and then was marked as read.
        /// </summary>
        TappedNotification = 1,

        /// <summary>
        /// The user opened the notifications screen, selected the notification, and marked it as read.
        /// </summary>
        NotificationScreen = 2,

        /// <summary>
        /// The user selected the mark all option in the notifications screen, marking many notifications at once.
        /// </summary>
        Batch = 3,
    }
}
