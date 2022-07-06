using NotificationCenter.Core.Domain;
using System;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Contracts
{
    /// <summary>
    /// Represents the IAppServiceContract
    /// </summary>
    public interface IAppService
    {
        /// <summary>
        /// Gets an app by its name
        /// </summary>
        /// <param name="appName">The name of the app to search for</param>
        /// <returns>An <see cref="App"/> with the provided name if found.</returns>
        Task<App> GetAppByName(string appName);

        /// <summary>
        /// Gets an app by its Id
        /// </summary>
        /// <param name="appId">The Id of the app to search for</param>
        /// <returns>An <see cref="App"/> with the provided Id if found.</returns>
        Task<App> GetAppById(Guid appId);
    }
}
