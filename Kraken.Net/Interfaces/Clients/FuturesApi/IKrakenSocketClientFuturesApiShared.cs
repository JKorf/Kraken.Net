using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Shared interface for Futures socket API usage
    /// </summary>
    public interface IKrakenSocketClientFuturesApiShared
        : ITickerSocketClient,
        ITradeSocketClient,
        IBookTickerSocketClient,
        IBalanceSocketClient,
        IFuturesOrderSocketClient,
        IUserTradeSocketClient,
        IPositionSocketClient
    {
    }
}
