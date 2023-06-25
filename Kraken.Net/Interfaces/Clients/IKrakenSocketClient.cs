using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Kraken websocket API. 
    /// </summary>
    public interface IKrakenSocketClient : ISocketClient
    {
        /// <summary>
        /// Spot streams
        /// </summary>
        IKrakenSocketClientSpotApi SpotApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}