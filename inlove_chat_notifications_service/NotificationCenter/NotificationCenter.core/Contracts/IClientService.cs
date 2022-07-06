using NotificationCenter.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Contracts
{
    /// <summary>
    /// Represents a contract for the service of this app.
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// Retrieves a list of client registered in the app.
        /// </summary>
        /// <returns>list of clients</returns>
        IList<Client> GetClients();

        /// <summary>
        /// Retrives a client by id.
        /// </summary>
        /// <param name="id">Client's id.</param>
        /// <returns></returns>
        Client GetClientById(int id);

        /// <summary>
        /// Retrieves a client by username.
        /// </summary>
        /// <param name="username">client username</param>
        /// <returns>specific client</returns>
        Task<Client> GetClientByUsername(string username);

        /// <summary>
        /// Retrieves a specific client by connectionId
        /// </summary>
        /// <param name="connectionId">client connection id of Signalr</param>
        /// <returns>a specific client</returns>
        Task<Client> GetClientByConnectionId(string connectionId);

        /// <summary>
        ///  Register a client.
        /// </summary>
        /// <param name="client">client for register</param>
        Task CreateClient(Client client);

        /// <summary>
        /// Evaluate if client is connected.
        /// </summary>
        /// <param name="connectionId">client connectionId</param>
        /// <returns></returns>
        Task ClientIsConnected(string connectionId);

        /// <summary>
        /// Evaluate if client is disconnected
        /// </summary>
        /// <param name="connectionId">client connectionId</param>
        /// <returns></returns>
        Task ClientIsDisconnected(string connectionId);

        /// <summary>
        /// update a client
        /// </summary>
        /// <param name="client">client for update</param>
        Task UpdateClient(Client client);

    }
}
