using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core
{
    ///<inheritdoc cref="ITokenUserStoreService"/>
    public class TokenUserStoreService : ITokenUserStoreService
    {
        private readonly NotificationCenterContext _context;

        /// <summary>
        /// Creates a new instance of TokenUserSevrice
        /// </summary>
        /// <param name="context">An instance <see cref="NotificationCenterContext"/></param>
        public TokenUserStoreService(NotificationCenterContext context)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public async Task<TokenUser> GetTokenUserByUserId(string userId)
        {
            var user = await _context.TokenUsers.FirstOrDefaultAsync(t => t.UserId == userId);

            return user;
        }

        ///<inheritdoc/>
        public async Task<bool> CheckIfUserExistsWithFcmToken(string FCMToken)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.FcmToken == FCMToken);

            if (device == null)
            {
                return false;
            }

            var tokenUser = await GetTokenUser(device.TokenUserId);

            return tokenUser != null;
        }

        ///<inheritdoc/>
        public async Task<TokenUser> GetTokenUser(Guid tokenUserId)
        {
            TokenUser result = await _context.TokenUsers.FindAsync(tokenUserId);

            return result;
        }

        ///<inheritdoc/>
        public async Task<TokenUser> AddTokenUser(TokenUser tokenUser)
        {
            var result = await _context.TokenUsers.AddAsync(tokenUser);

            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
