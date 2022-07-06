using Microsoft.EntityFrameworkCore;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using System;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Services
{
    internal class AppService : IAppService
    {
        private readonly NotificationCenterContext _context;

        /// <summary>
        /// Creates a new instance of the app service
        /// </summary>
        /// <param name="context">An instance of <see cref="NotificationCenterContext"/></param>
        public AppService(NotificationCenterContext context)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public async Task<App> GetAppById(Guid appId)
        {
            return await _context.Apps.FindAsync(appId);
        }

        ///<inheritdoc/>
        public async Task<App> GetAppByName(string appName)
        {
            return await _context.Apps.FirstOrDefaultAsync(app => app.AppName.ToLower() == appName.ToLower());
        }
    }
}
