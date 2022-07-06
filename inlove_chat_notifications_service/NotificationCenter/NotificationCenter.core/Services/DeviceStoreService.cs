using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Enums;
using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationCenter.Core
{
    ///<inheritdoc cref="IDeviceStoreService"/>
    public class DeviceStoreService : IDeviceStoreService
    {
        private readonly NotificationCenterContext _context;

        /// <summary>
        /// Creates a new instance of device service
        /// </summary>
        /// <param name="context">An instance of <see cref="NotificationCenterContext"/></param>
        public DeviceStoreService(NotificationCenterContext context) => _context = context;

        ///<inheritdoc/>
        public async Task<Device> AddNewDevice(Device device)
        {
            EntityEntry<Device> entry = await _context.AddAsync(device);

            entry.State = EntityState.Added;

            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        ///<inheritdoc/>
        public async Task<Device> GetDeviceByIdentifier(string identifier, string appName)
        {
            Device found = await _context.Devices
                .Include(device => device.App)
                .FirstOrDefaultAsync(device => device.DeviceIdentifier == identifier && device.App.AppName.ToLower() == appName.ToLower());

            return found;
        }

        ///<inheritdoc/>
        public async Task<ISet<Device>> GetDevicesForUserByUserId(string userId)
        {
            TokenUser tokenUser = await _context.TokenUsers.FirstOrDefaultAsync(t => t.UserId == userId);

            if (tokenUser == null) return null;

            List<Device> devices = await _context.Devices.Where(d => d.TokenUserId == tokenUser.Id).ToListAsync();

            return devices.ToHashSet();
        }

        ///<inheritdoc/>
        public async Task<ISet<Device>> GetDevicesForUserByTokenUserId(Guid tokenUserId)
        {
            List<Device> devices = await _context.Devices
                .Where(d => d.TokenUserId == tokenUserId)
                .Include(d => d.App)
                .ToListAsync();

            return devices.ToHashSet();
        }

        ///<inheritdoc/>
        public Dictionary<int, string> GetDeviceTypes()
        {
            IEnumerable<DeviceType> types = Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>();

            var dictionary = new Dictionary<int, string>();

            foreach (var type in types)
            {
                dictionary.Add((int)type, type.ToString());
            }

            return dictionary;
        }

        ///<inheritdoc/>
        public Task RemoveDevice(Device device)
        {
            _context.Entry(device).State = EntityState.Deleted;

            return _context.SaveChangesAsync();
        }

        ///<inheritdoc/>
        public async Task<Device> UpdateDeviceTokens(UserTokenRequest updateRequest)
        {
            var foundDevice = await GetDeviceByIdentifier(updateRequest.DeviceIdentifier, updateRequest.AppName);

            if (foundDevice == null) return null;

            string fcmToken = updateRequest.DeviceToken ?? foundDevice.FcmToken;
            string apnsToken = updateRequest.ApnsToken ?? foundDevice.ApnsToken;

            foundDevice.FcmToken = fcmToken;
            foundDevice.ApnsToken = apnsToken;

            _context.Entry(foundDevice).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return foundDevice;
        }
    }
}
