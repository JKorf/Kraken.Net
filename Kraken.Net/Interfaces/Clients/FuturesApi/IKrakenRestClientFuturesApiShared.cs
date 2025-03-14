using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Shared interface for Futures rest API usage
    /// </summary>
    public interface IKrakenRestClientFuturesApiShared : 
        IBalanceRestClient,
        IKlineRestClient,
        IOrderBookRestClient,
        IRecentTradeRestClient,
        IFundingRateRestClient,
        IFuturesSymbolRestClient,
        IFuturesTickerRestClient,
        IMarkPriceKlineRestClient,
        IOpenInterestRestClient,
        ILeverageRestClient,
        IFuturesOrderRestClient,
        IFeeRestClient,
        IFuturesOrderClientIdClient
    {
    }
}
