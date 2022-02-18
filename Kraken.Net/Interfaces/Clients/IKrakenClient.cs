using CryptoExchange.Net.Interfaces;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Kraken API. 
    /// </summary>
    public interface IKrakenClient : IRestClient
    {
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        IKrakenClientSpotApi SpotApi { get; }

    }
}