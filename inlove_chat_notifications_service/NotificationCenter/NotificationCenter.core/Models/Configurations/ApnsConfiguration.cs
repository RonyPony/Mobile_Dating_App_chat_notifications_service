using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using dotAPNS;

namespace NotificationCenter.Core.Models.Configurations
{
    /// <summary>
    /// Represents a class containing the certificate configuration for APNS 
    /// </summary>
    public class ApnsConfiguration
    {
        public readonly ApnsClient Apns;

        /// <summary>
        /// Creates a new instance of the APNS Configuration
        /// </summary>
        /// <param name="fileName">The Path of the file with the certificate</param>
        /// <param name="configurations">A class with several configurations</param>
        public ApnsConfiguration(string fileName, AppSettingsConfigurations configurations)
        {
            if (fileName.EndsWith(".p8"))
            {
                var options = new ApnsJwtOptions()
                {
                    BundleId = configurations.IosBundleId,
                    CertFilePath = fileName,
                    KeyId = configurations.AppleKeyId,
                    TeamId = configurations.AppleTeamId
                };
                Apns = ApnsClient.CreateUsingJwt(new HttpClient(), options).UseSandbox();
            }
            else
            {
                Apns = ApnsClient.CreateUsingCert(fileName);
            }
        }
    }
}
