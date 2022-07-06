using Microsoft.EntityFrameworkCore;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Services
{
    /// <summary>
    /// Represents client services implementation for the app.
    /// </summary>
    public class ClientService : IClientService
    {
        #region
        private readonly NotificationCenterContext _notificationCenterContext;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates an instance of <see cref="CustomerController"/>
        /// </summary>
        /// <param name="notificationCenterContext">An implementation of <see cref="NotificationCenterContext"/>.</param>
        public ClientService(NotificationCenterContext notificationCenterContext)
        {
            _notificationCenterContext = notificationCenterContext;
        }

        #endregion

        #region Methods
        ///<inheritdoc/>
        public async Task ClientIsConnected(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentException("InvalidConnectionId");

            Client client = await GetClientByConnectionId(connectionId);

            if (client != null)
            {
                client.IsConnected = true;
                await UpdateClient(client);
            }
        }

        ///<inheritdoc/>
        public async Task ClientIsDisconnected(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentException("InvalidConnectionId");

            Client client = await GetClientByConnectionId(connectionId);

            if (client != null)
            {
                client.IsConnected = false;
                await UpdateClient(client);
            }
        }

        ///<inheritdoc/>
        public async Task CreateClient(Client client)
        {
            if (client is null)
                throw new ArgumentException("InvalidClientRequest");

            client.IsConnected = true;
            client.CreatedAt = DateTime.Now;
            _notificationCenterContext.Clients.Add(client);
            await _notificationCenterContext.SaveChangesAsync();
        }

        ///<inheritdoc/>
        public async Task<Client> GetClientByConnectionId(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentException("InvalidConnectionId");

            return await _notificationCenterContext.Clients
                .Include(client => client.ClientAndRooms)
                .FirstOrDefaultAsync(client => client.ConnectionId.Equals(connectionId));
        }

        public Client GetClientById(int id)
        {
            if (id == 0)
                throw new ArgumentException("InvalidClientId");

            return _notificationCenterContext.Clients
                .FirstOrDefault(client => client.Id == id);
        }

        ///<inheritdoc/>
        public async Task<Client> GetClientByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("InvalidUsername");
         
            return await _notificationCenterContext.Clients
                .Include(client => client.ClientAndRooms)
                .FirstOrDefaultAsync(client => client.Username.Equals(username));
        }

        ///<inheritdoc/>
        public IList<Client> GetClients()
        {
            return _notificationCenterContext.Clients
                .Include(client => client.ClientAndRooms)
                .ToList();
        }

        ///<inheritdoc/>
        public async Task UpdateClient(Client client)
        {
            if (client is null)
                throw new ArgumentException("InvalidClientRequest");

            _notificationCenterContext.Clients.Update(client);
            await _notificationCenterContext.SaveChangesAsync();
        }

        #endregion
    }
}
