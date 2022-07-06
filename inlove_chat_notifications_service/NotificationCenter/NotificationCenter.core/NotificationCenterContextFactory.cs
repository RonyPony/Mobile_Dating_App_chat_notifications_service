using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NotificationCenter.Core
{
    /// <summary>
    /// A <see cref="IDesignTimeDbContextFactory{}"/> used at design time to create migrations.
    /// </summary>
    public class NotificationCenterContextFactory : IDesignTimeDbContextFactory<NotificationCenterContext>
    {
        ///<inheritdoc/>
        public NotificationCenterContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("designSettings.json")
                .Build();
            // Here we create the DbContextOptionsBuilder manually.        
            var builder = new DbContextOptionsBuilder<NotificationCenterContext>();

            // Build connection string. This requires that you have a connectionstring in the appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            // Create our DbContext.
            return new NotificationCenterContext(builder.Options);
        }
    }
}
