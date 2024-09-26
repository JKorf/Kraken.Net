using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Shared interface for Futures socket API usage
    /// </summary>
    public interface IKrakenSocketClientFuturesApiShared
        : ISharedClient
        //Can be implemented with V2 websockets
    {
    }
}
