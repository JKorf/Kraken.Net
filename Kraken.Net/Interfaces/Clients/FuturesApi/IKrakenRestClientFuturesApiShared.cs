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
        IGetBalancesRest,
        IGetKlinesRest,
        IGetOrderBookRest,
        IGetRecentTradesRest,
        IGetBookTickerRest,
        IGetFundingRateHistoryRest,
        IGetFuturesSymbolsRest,
        IGetTickerRest,
        IGetAllTickersRest,
        IGetMarkPriceKlinesRest,
        IGetOpenInterestRest,
        IGetLeverageRest,
        ISetLeverageRest,
        IPlaceFuturesOrderRest,
        IGetFuturesOrderRest,
        IGetOpenFuturesOrdersRest,
        IGetFuturesUserTradeHistoryRest,
        ICancelFuturesOrderRest,
        IGetPositionsRest,
        IGetFeesRest,
        IGetFuturesOrderByClientOrderIdRest,
        ICancelFuturesOrderByClientOrderIdRest,
        ISetFuturesTpSlRest,
        ICancelFuturesTpSlRest
    {
    }
}
