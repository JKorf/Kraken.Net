using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot socket API usage
    /// </summary>
    public interface IKrakenSocketClientSpotApiShared : ISharedClient
        //ITickerSocketClient,
        //ITradeSocketClient,
        //IBookTickerSocketClient,
        //IKlineSocketClient
        //Can be implemented with V2 websockets
        //IBalanceSocketClient, 
        //ISpotOrderSocketClient
    {
    }
}
