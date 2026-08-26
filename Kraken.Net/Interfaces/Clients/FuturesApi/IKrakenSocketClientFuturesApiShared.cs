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

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IKrakenSocketClientFuturesSharedApi
        : ISubscribeTickerOperation,
        ISubscribeTradesOperation,
        ISubscribeBookTickerOperation,
        ISubscribeBalancesOperation,
        ISubscribeFuturesOrdersOperation,
        ISubscribeUserTradesOperation,
        ISubscribePositionsOperation
    {
    }
}
