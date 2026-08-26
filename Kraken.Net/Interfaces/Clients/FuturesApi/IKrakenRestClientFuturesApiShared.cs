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
        IFuturesOrderClientIdRestClient,
        IFuturesTpSlRestClient,
        IBookTickerRestClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IKrakenRestClientFuturesSharedApi :
        IGetBalancesEndpoint,
        IGetKlinesEndpoint,
        IGetOrderBookEndpoint,
        IGetRecentTradesEndpoint,
        IGetBookTickerEndpoint,
        IGetFundingRateHistoryEndpoint,
        IGetFuturesSymbolsEndpoint,
        IGetFuturesTickerEndpoint,
        IGetAllFuturesTickersEndpoint,
        IGetMarkPriceKlinesEndpoint,
        IGetOpenInterestEndpoint,
        IGetLeverageEndpoint,
        ISetLeverageEndpoint,
        IPlaceFuturesOrderEndpoint,
        IGetFuturesOrderEndpoint,
        IGetOpenFuturesOrdersEndpoint,
        IGetFuturesUserTradeHistoryEndpoint,
        ICancelFuturesOrderEndpoint,
        IGetPositionsEndpoint,
        IClosePositionEndpoint,
        IGetFeesEndpoint,
        IGetFuturesOrderByClientOrderIdEndpoint,
        ICancelFuturesOrderByClientOrderIdEndpoint,
        ISetFuturesTpSlEndpoint,
        ICancelFuturesTpSlEndpoint
    {
    }
}
