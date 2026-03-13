using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Options;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Kraken websocket API. 
    /// </summary>
    public interface IKrakenSocketClient : ISocketClient<KrakenCredentials>
    {
        /// <summary>
        /// Spot Api
        /// </summary>
        /// <see cref="IKrakenSocketClientSpotApi"/>
        IKrakenSocketClientSpotApi SpotApi { get; }
        /// <summary>
        /// Futures Api
        /// </summary>
        /// <see cref="IKrakenSocketClientFuturesApi"/>
        IKrakenSocketClientFuturesApi FuturesApi { get; }
    }
}