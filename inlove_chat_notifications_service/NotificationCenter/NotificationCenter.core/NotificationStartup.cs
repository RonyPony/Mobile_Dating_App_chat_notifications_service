using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Managers;
using NotificationCenter.Core.Models.Configurations;
using NotificationCenter.Core.Services;
using System.IO;
using System.Reflection;

namespace NotificationCenter.Core
{
    /// <summary>
    /// Initializes the required components.
    /// </summary>
    public static class NotificationStartup
    {
        //TODO: Document this in readme.MD instructions
        private static readonly string AppSettingsKeyName = "NotificationCenter";

        /// <summary>
        /// Registers all of the dependencies needed to run the notification center
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configFunction">A function that returns an object, that contains the required configuration parameters</param>
        /// <param name="onlyReigsterManagers">If true the User must supply an implementation for the app provider</param>
        public static void AddNotificationCenter(this IServiceCollection services, IConfiguration config, bool onlyReigsterManagers = false)
        {
            var libConfiguration = GetConfigurationFromAppSettings(config);

            RegisterFirebaseDependencies(services, libConfiguration.FirebaseJsonFile);

            RegisterVoipNotificationDependencies(services, libConfiguration);

            RegisterDependencies(services, onlyReigsterManagers);
        }

        /// <summary>
        /// Registers the context for the notification center, requires SQL Server TO run
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <param name="config">Configurations</param>
        /// <param name="migrationAssembly">The assembly that will run the migrations</param>
        public static void AddNotificationCenterContext(this IServiceCollection services, IConfiguration config, Assembly migrationAssembly)
        {
            var libConfiguration = GetConfigurationFromAppSettings(config);

            var assembly = migrationAssembly.GetName().Name;

            var connectionString = config.GetConnectionString(libConfiguration.DatabaseConnectionStringName);
            services.AddDbContext<NotificationCenterContext>(options
                => options.UseSqlServer(connectionString, b
                    =>
                {
                    b.MigrationsAssembly(assembly);
                    b.MigrationsHistoryTable("_WNC_HistoryTable");
                }));
        }

        private static void RegisterFirebaseDependencies(IServiceCollection services, string path)
        {
            string filePath = GetPathForGivenFile(path);

            var configurationJson = File.ReadAllText(filePath);

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions { Credential = GoogleCredential.FromJson(configurationJson) });
            }

            var instance = FirebaseMessaging.DefaultInstance;

            services.AddSingleton(typeof(FirebaseMessaging), instance);
        }

        private static void RegisterVoipNotificationDependencies(IServiceCollection services, AppSettingsConfigurations config)
        {
            string filePath = GetPathForGivenFile(config.VoipCertificateFile);

            var apnsConfig = new ApnsConfiguration(filePath, config);

            services.AddSingleton(typeof(ApnsConfiguration), apnsConfig);
        }

        private static void RegisterDependencies(IServiceCollection services, bool onlyManagers)
        {
            //Services registration
            if (!onlyManagers)
            {
                services.AddScoped<ITokenUserStoreService, TokenUserStoreService>();
                services.AddScoped<IDeviceStoreService, DeviceStoreService>();
                services.AddScoped<ITemplateTypeStoreService, TemplateTypeStoreService>();
                services.AddScoped<INotificationTemplateStoreService, NotificationTemplateStoreService>();
                services.AddScoped<IClientService, ClientService>();
                services.AddScoped<IRoomService, RoomService>();
                services.AddScoped<IChatRoomMessageService, ChatRoomMessageService>();
                services.AddScoped<IAppService, AppService>();
            }
            services.AddScoped<TemplateTypeManager, TemplateTypeManager>();
            services.AddScoped<NotificationTemplateManager, NotificationTemplateManager>();
            services.AddScoped<TokenManager, TokenManager>();
        }

        private static string GetPathForGivenFile(string path)
        {
            if (path.Contains('/'))
            {
                return path;
            }

            var route = Directory.GetCurrentDirectory();
            return Path.Join(route, path);
        }

        private static AppSettingsConfigurations GetConfigurationFromAppSettings(IConfiguration config)
            => config.GetSection(AppSettingsKeyName).Get<AppSettingsConfigurations>();
    }
}