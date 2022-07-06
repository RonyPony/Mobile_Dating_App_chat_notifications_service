namespace NotificationCenter.Core.Enums
{
    /// <summary>
    /// Represents the apptype enum of the application where the client registered
    /// </summary>
    public enum AppType
    {
        // This type is used when any type is selected
        None = 0,

        //This type is used when the notification is going to the Client app
        ClientApp = 1,

        ////This type is used when the notification is going to the Driver app
        DriverApp = 2
    }
}
