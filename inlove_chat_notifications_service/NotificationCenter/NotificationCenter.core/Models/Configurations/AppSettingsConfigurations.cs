namespace NotificationCenter.Core.Models.Configurations
{
    /// <summary>
    /// Represents the configuration parameters to be taken from the appsettings.json
    /// </summary>
    public class AppSettingsConfigurations
    {
        /// <summary>
        /// Represents the connection string to the database
        /// TODO: Temporal implementation
        /// </summary>
        public string DatabaseConnectionStringName { get; set; }

        /// <summary>
        /// Represents the name of the Admin configuration file
        /// </summary>
        public string FirebaseJsonFile { get; set; } = "firebase_admin.json";

        /// <summary>
        /// Represents the name of the VoipCertificate
        /// </summary>
        public string VoipCertificateFile { get; set; }

        /// <summary>
        /// Represents the bundle Id for iOS app
        /// </summary>
        public string IosBundleId { get; set; }

        /// <summary>
        /// Represents the Apple Key Id
        /// </summary>
        public string AppleKeyId { get; set; }

        /// <summary>
        /// Represents the Apple Team ID
        /// </summary>
        public string AppleTeamId { get; set; }
    }
}
