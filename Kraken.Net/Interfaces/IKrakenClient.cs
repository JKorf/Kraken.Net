using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Kraken.Net.Converters;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Socket;

namespace Kraken.Net.Interfaces
{
    /// <summary>
    /// Interface for Kraken Rest API
    /// </summary>
    public interface IKrakenClient: IRestClient
    {
        /// <summary>
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        void SetApiCredentials(string apiKey, string apiSecret);

        /// <summary>
        /// Get the server time
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Server time</returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get a list of assets and info about them
        /// </summary>
        /// <param name="assets">Filter list for specific assets</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary of asset info</returns>
        Task<WebCallResult<Dictionary<string, KrakenAssetInfo>>> GetAssetsAsync(IEnumerable<string>? assets = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of symbols and info about them
        /// </summary>
        /// <param name="symbols">Filter list for specific symbols</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary of symbol info</returns>
        Task<WebCallResult<Dictionary<string, KrakenSymbol>>> GetSymbolsAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default);

        /// <summary>
        /// Get tickers for symbol
        /// </summary>
        /// <param name="symbol">Symbol to get tickers for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ticker info</returns>
        Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get tickers for symbols
        /// </summary>
        /// <param name="symbols">Symbols to get tickers for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ticker info</returns>
        Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

        /// <summary>
        /// Gets kline data for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get data for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="since">Return klines since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Kline data</returns>
        Task<WebCallResult<KrakenKlinesResult>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? since = null, CancellationToken ct = default);

        /// <summary>
        /// Get the order book for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get the book for</param>
        /// <param name="limit">Limit to book to the best x bids/asks</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        Task<WebCallResult<KrakenOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of recent trades for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get trades for</param>
        /// <param name="since">Return trades since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Recent trades</returns>
        Task<WebCallResult<KrakenTradesResult>> GetTradeHistoryAsync(string symbol, DateTime? since = null, CancellationToken ct = default);

        /// <summary>
        /// Get spread data for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get spread data for</param>
        /// <param name="since">Return spread data since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Spread data</returns>
        Task<WebCallResult<KrakenSpreadsResult>> GetRecentSpreadAsync(string symbol, DateTime? since = null, CancellationToken ct = default);

        /// <summary>
        /// Get balances
        /// </summary>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with balances for assets</returns>
        Task<WebCallResult<Dictionary<string, decimal>>> GetBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade balance
        /// </summary>
        /// <param name="baseAsset">Base asset to get trade balance for</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade balance data</returns>
        Task<WebCallResult<KrakenTradeBalance>> GetTradeBalanceAsync(string? baseAsset = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of open orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        Task<WebCallResult<OpenOrdersPage>> GetOpenOrdersAsync(string? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of closed orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Closed orders page</returns>
        Task<WebCallResult<KrakenClosedOrdersPage>> GetClosedOrdersAsync(string? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific order
        /// </summary>
        /// <param name="clientOrderId">Get orders by clientOrderId</param>
        /// <param name="orderId">Get order by its order id</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with order info</returns>
        Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrderAsync(string? orderId = null, string? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific orders
        /// </summary>
        /// <param name="clientOrderId">Get orders by clientOrderId</param>
        /// <param name="orderIds">Get orders by their order ids</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with order info</returns>
        Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrdersAsync(IEnumerable<string>? orderIds = null, string? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade history
        /// </summary>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade history page</returns>
        Task<WebCallResult<KrakenUserTradesPage>> GetUserTradeHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific trades
        /// </summary>
        /// <param name="tradeId">The trade to get info on</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with trade info</returns>
        Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(string tradeId, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific trades
        /// </summary>
        /// <param name="tradeIds">The trades to get info on</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with trade info</returns>
        Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(IEnumerable<string> tradeIds, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of open positions
        /// </summary>
        /// <param name="transactionIds">Filter by transaction ids</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with position info</returns>
        Task<WebCallResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(IEnumerable<string>? transactionIds = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get ledger entries info
        /// </summary>
        /// <param name="assets">Filter list by asset names</param>
        /// <param name="entryTypes">Filter list by entry types</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Ledger entries page</returns>
        Task<WebCallResult<KrakenLedgerPage>> GetLedgerInfoAsync(IEnumerable<string>? assets = null, IEnumerable<LedgerEntryType>? entryTypes = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific ledger entries
        /// </summary>
        /// <param name="ledgerIds">The ids to get info for</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ledger entry info</returns>
        Task<WebCallResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(IEnumerable<string>? ledgerIds = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade volume
        /// </summary>
        /// <param name="symbols">Symbols to get data for</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade fee info</returns>
        Task<WebCallResult<KrakenTradeVolume>> GetTradeVolumeAsync(IEnumerable<string>? symbols = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit methods
        /// </summary>
        /// <param name="asset">Asset to get methods for</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Array with methods for deposit</returns>
        Task<WebCallResult<IEnumerable<KrakenDepositMethod>>> GetDepositMethodsAsync(string asset, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit addresses for an asset
        /// </summary>
        /// <param name="asset">The asset to get the deposit address for</param>
        /// <param name="depositMethod">The method of deposit</param>
        /// <param name="generateNew">Whether to generate a new address</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenDepositAddress>>> GetDepositAddressesAsync(string asset, string depositMethod, bool generateNew = false, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get status deposits for an asset
        /// </summary>
        /// <param name="asset">Asset to get deposit info for</param>
        /// <param name="depositMethod">The deposit method</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit status list</returns>
        Task<WebCallResult<IEnumerable<KrakenDepositStatus>>> GetDepositStatusAsync(string asset, string depositMethod, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// </summary>
        /// <param name="symbol">The symbol the order is on</param>
        /// <param name="side">The side of the order</param>
        /// <param name="type">The type of the order</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="clientOrderId">A client id to reference the order by</param>
        /// <param name="price">Price of the order:
        /// Limit=limit price,
        /// StopLoss=stop loss price,
        /// TakeProfit=take profit price,
        /// StopLossProfit=stop loss price,
        /// StopLossProfitLimit=stop loss price,
        /// StopLossLimit=stop loss trigger price,
        /// TakeProfitLimit=take profit trigger price,
        /// TrailingStop=trailing stop offset,
        /// TrailingStopLimit=trailing stop offset,
        /// StopLossAndLimit=stop loss price,
        /// </param>
        /// <param name="secondaryPrice">Secondary price of an order:
        /// StopLossProfit/StopLossProfitLimit=take profit price,
        /// StopLossLimit/TakeProfitLimit=triggered limit price,
        /// TrailingStopLimit=triggered limit offset,
        /// StopLossAndLimit=limit price</param>
        /// <param name="leverage">Desired leverage</param>
        /// <param name="startTime">Scheduled start time</param>
        /// <param name="expireTime">Expiration time</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Placed order info</returns>
        Task<WebCallResult<KrakenPlacedOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal quantity,
            uint? clientOrderId = null,
            decimal? price = null,
            decimal? secondaryPrice = null,
            decimal? leverage = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            bool? validateOnly = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">The id of the order to cancel</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Cancel result</returns>
        Task<WebCallResult<KrakenCancelResult>> CancelOrderAsync(string orderId, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info before a withdrawal
        /// </summary>
        /// <param name="asset">The asset</param>
        /// <param name="key">The withdrawal key name</param>
        /// <param name="amount">The amount to withdraw</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenWithdrawInfo>> GetWithdrawInfoAsync(string asset, string key, decimal amount,
            string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Withdraw funds
        /// </summary>
        /// <param name="asset">The asset being withdrawn</param>
        /// <param name="key">The withdrawal key name, as set up on your account</param>
        /// <param name="amount">The amount to withdraw, including fees</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdraw reference id</returns>
        Task<WebCallResult<KrakenWithdraw>> WithdrawAsync(string asset, string key, decimal amount, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get the token to connect to the private websocket streams
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<KrakenWebSocketToken>> GetWebsocketTokenAsync(CancellationToken ct = default);
    }
}