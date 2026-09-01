using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot rest API usage
    /// </summary>
    public interface IKrakenRestClientSpotApiShared :
        IAssetsRestClient,
        IBalanceRestClient,
        IDepositRestClient,
        IKlineRestClient,
        IOrderBookRestClient,
        IRecentTradeRestClient,
        ISpotOrderRestClient,
        ISpotSymbolRestClient,
        ISpotTickerRestClient,
        //ITradeHistoryRestClient
        IWithdrawalRestClient,
        IWithdrawRestClient,
        IFeeRestClient,
        ISpotOrderClientIdRestClient,
        IBookTickerRestClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IKrakenRestClientSpotSharedApi :
        IGetAssetRest,
        IGetAllAssetsRest,
        IGetBalancesRest,
        IGetDepositHistoryRest,
        IGetDepositAddressesRest,
        IGetKlinesRest,
        IGetOrderBookRest,
        IGetRecentTradesRest,
        IPlaceSpotOrderRest,
        IGetSpotOrderRest,
        IGetSpotOrderByClientOrderIdRest,
        IGetOpenSpotOrdersRest,
        IGetClosedSpotOrdersRest,
        IGetSpotOrderTradesRest,
        IGetSpotUserTradeHistoryRest,
        ICancelSpotOrderRest,
        ICancelSpotOrderByClientOrderIdRest,
        IGetSpotSymbolsRest,
        IGetSpotTickerRest,
        IGetAllSpotTickersRest,
        IGetWithdrawalHistoryRest,
        IWithdrawRest,
        IGetFeesRest,
        IGetBookTickerRest
    {
    }
}
