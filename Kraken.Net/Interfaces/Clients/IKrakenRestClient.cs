using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Options;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Kraken API. 
    /// </summary>
    public interface IKrakenRestClient : IRestClient<KrakenCredentials>
    {
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="IKrakenRestClientSpotApi"/>
        IKrakenRestClientSpotApi SpotApi { get; }

        /// <summary>
        /// Futures API endpoints
        /// </summary>
        /// <see cref="IKrakenRestClientFuturesApi"/>
        IKrakenRestClientFuturesApi FuturesApi { get; }
    }
}