using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Models;

namespace NotificationCenter.Core.Contracts
{
    /// <summary>
    /// Represents the data access methods for devices
    /// </summary>
    public interface IDeviceStoreService
    {
        /// <summary>
        /// Adds a new  <see cref="Device"/>.
        /// </summary>
        /// <param name="device">The device to be added</param>
        /// <returns>The device added.</returns>
        Task<Device> AddNewDevice(Device device);

        /// <summary>
        /// Gets a device from an identifier
        /// </summary>
        /// <param name="device">An instance <see cref="Device"/></param>
        /// <returns>A <see cref="Device"/></returns>
        Task<Device> GetDeviceByIdentifier(string identifier, string appName);

        /// <summary>
        /// Gets devices for a User
        /// <param name="userId">An <see cref="string"/> value representing the userID</param>
        /// </summary>
        Task<ISet<Device>> GetDevicesForUserByUserId(string userId);

        /// <summary>
        /// Gets all devices associated to a user by the token user iD
        /// </summary>
        /// <param name="tokenUserId">An <see cref="Guid"/> value, representing the ID of the USER</param>
        /// <returns></returns>
        Task<ISet<Device>> GetDevicesForUserByTokenUserId(Guid tokenUserId);

        /// <summary>
        /// Gets a keyValuePair collection with the values of the device type ENUM
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetDeviceTypes();

        /// <summary>
        /// Serves to remove a device no longer in use
        /// </summary>
        /// <param name="device">An <see cref="Device"/> object</param>
        /// <returns></returns>
        Task RemoveDevice(Device device);

        /// <summary>
        /// Updates the token or tokens for a given device
        /// </summary>
        /// <param name="updateRequest">A <see cref="UserTokenRequest"/> object</param>
        /// <returns>A <see cref="Device"/> object</returns>
        Task<Device> UpdateDeviceTokens(UserTokenRequest updateRequest);
    }
}
