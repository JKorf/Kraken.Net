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
        IKrakenSocketClientSpotStreams SpotStreams { get; }
    }
}