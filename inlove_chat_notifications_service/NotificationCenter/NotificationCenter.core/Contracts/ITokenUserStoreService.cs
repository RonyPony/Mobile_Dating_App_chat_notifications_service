using System;
using System.Threading.Tasks;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Contracts
{
    /// <summary>
    /// Represents the ITokenService Contract
    /// </summary>
    public interface ITokenUserStoreService
    {
        /// <summary>
        /// Finds whether the user exists with the registered FCM Token
        /// </summary>
        /// <param name="FCMToken"></param>
        /// <returns></returns>
        Task<bool> CheckIfUserExistsWithFcmToken(string FCMToken);

        /// <summary>
        /// Finds whether the token user exists in the database
        /// </summary>
        /// <param name="tokenUserId">Represents the ID of the TOKEN user</param>
        /// <returns>A <see cref="bool"/> value</returns>
        Task<TokenUser> GetTokenUserByUserId(string userId);

        /// <summary>
        /// Returns the found token user
        /// </summary>
        /// <param name="tokenUserId">A <see cref="Guid"/> representing the token user id</param>
        /// <returns>A <see cref="TokenUser"/> instance</returns>
        Task<TokenUser> GetTokenUser(Guid tokenUserId);

        /// <summary>
        /// Adds a new token user to the database
        /// </summary>
        /// <param name="tokenUser">A <see cref="TokenUser"/> item</param>
        /// <returns>A <see cref="TokenUser"/> instance</returns>
        Task<TokenUser> AddTokenUser(TokenUser tokenUser);
    }
}
