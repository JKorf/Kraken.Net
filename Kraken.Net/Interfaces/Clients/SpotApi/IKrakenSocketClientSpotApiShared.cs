using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot socket API usage
    /// </summary>
    public interface IKrakenSocketClientSpotApiShared :
        ITickerSocketClient,
        ITradeSocketClient,
        IBookTickerSocketClient,
        IKlineSocketClient,
        IBalanceSocketClient,
        ISpotOrderSocketClient
    {
    }
}
