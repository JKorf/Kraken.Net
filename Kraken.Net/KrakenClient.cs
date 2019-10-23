using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Kraken.Net.Converters;
using Kraken.Net.Interfaces;
using Kraken.Net.Objects;
using Newtonsoft.Json;

namespace Kraken.Net
{
    /// <summary>
    /// Client for the Kraken Rest API
    /// </summary>
    public class KrakenClient: RestClient, IKrakenClient
    {
        #region fields
        private static KrakenClientOptions defaultOptions = new KrakenClientOptions();
        private static KrakenClientOptions DefaultOptions => defaultOptions.Copy<KrakenClientOptions>();
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of KrakenClient using the default options
        /// </summary>
        public KrakenClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of KrakenClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public KrakenClient(KrakenClientOptions options) : base(options, options.ApiCredentials == null ? null : new KrakenAuthenticationProvider(options.ApiCredentials))
        {
            postParametersPosition = PostParameters.InBody;
            requestBodyFormat = RequestBodyFormat.FormData;
        }
        #endregion

        #region methods
        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="options"></param>
        public static void SetDefaultOptions(KrakenClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        public void SetApiCredentials(string apiKey, string apiSecret)
        {
            SetAuthenticationProvider(new KrakenAuthenticationProvider(new ApiCredentials(apiKey, apiSecret)));
        }

        /// <summary>
        /// Get the server time
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Server time</returns>
        public CallResult<DateTime> GetServerTime(CancellationToken ct = default) => GetServerTimeAsync(ct).Result;
        /// <summary>
        /// Get the server time
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Server time</returns>
        public async Task<CallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var result = await Execute<KrakenServerTime>(GetUri("/0/public/Time"), HttpMethod.Get, ct).ConfigureAwait(false);
            if (!result)
                return WebCallResult<DateTime>.CreateErrorResult(result.Error!);
            return new CallResult<DateTime>(result.Data.UnixTime, null);
        }

        /// <summary>
        /// Get a list of assets and info about them
        /// </summary>
        /// <param name="assets">Filter list for specific assets</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary of asset info</returns>
        public WebCallResult<Dictionary<string, KrakenAssetInfo>> GetAssets(CancellationToken ct = default, params string[] assets) => GetAssetsAsync(ct, assets).Result;
        /// <summary>
        /// Get a list of assets and info about them
        /// </summary>
        /// <param name="assets">Filter list for specific assets</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary of asset info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenAssetInfo>>> GetAssetsAsync(CancellationToken ct = default, params string[] assets)
        {
            var parameters = new Dictionary<string, object>();
            if(assets.Any())
                parameters.AddOptionalParameter("asset", string.Join(",", assets));

            return await Execute<Dictionary<string, KrakenAssetInfo>>(GetUri("/0/public/Assets"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of symbols and info about them
        /// </summary>
        /// <param name="symbols">Filter list for specific symbols</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary of symbol info</returns>
        public WebCallResult<Dictionary<string, KrakenSymbol>> GetSymbols(CancellationToken ct = default, params string[] symbols) => GetSymbolsAsync(ct, symbols).Result;
        /// <summary>
        /// Get a list of symbols and info about them
        /// </summary>
        /// <param name="symbols">Filter list for specific symbols</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary of symbol info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenSymbol>>> GetSymbolsAsync(CancellationToken ct = default, params string[] symbols)
        {
            var parameters = new Dictionary<string, object>();
            if (symbols.Any())
                parameters.AddOptionalParameter("pair", string.Join(",", symbols));

            return await Execute<Dictionary<string, KrakenSymbol>>(GetUri("/0/public/AssetPairs"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get tickers for symbols
        /// </summary>
        /// <param name="symbols">Symbols to get tickers for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ticker info</returns>
        public WebCallResult<Dictionary<string, KrakenRestTick>> GetTickers(CancellationToken ct = default, params string[] symbols) => GetTickersAsync(ct, symbols).Result;
        /// <summary>
        /// Get tickers for symbols
        /// </summary>
        /// <param name="symbols">Symbols to get tickers for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ticker info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(CancellationToken ct = default, params string[] symbols)
        {
            if (!symbols.Any())
                throw new ArgumentException("No symbols defined to get ticker data for");
                
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("pair", string.Join(",", symbols));

            return await Execute<Dictionary<string, KrakenRestTick>>(GetUri("/0/public/Ticker"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets kline data for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get data for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="since">Return klines since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Kline data</returns>
        public WebCallResult<KrakenKlinesResult> GetKlines(string symbol, KlineInterval interval, DateTime? since = null, CancellationToken ct = default) =>
            GetKlinesAsync(symbol, interval, since, ct).Result;
        /// <summary>
        /// Gets kline data for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get data for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="since">Return klines since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Kline data</returns>
        public async Task<WebCallResult<KrakenKlinesResult>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? since = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
                {"interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}
            };
            parameters.AddOptionalParameter("since", since.HasValue ? JsonConvert.SerializeObject(since, new TimestampSecondsConverter()): null);
            return await Execute<KrakenKlinesResult>(GetUri("/0/public/OHLC"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the order book for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get the book for</param>
        /// <param name="limit">Limit to book to the best x bids/asks</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        public WebCallResult<KrakenOrderBook> GetOrderBook(string symbol, int? limit = null, CancellationToken ct = default) => 
            GetOrderBookAsync(symbol, limit, ct).Result;
        /// <summary>
        /// Get the order book for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get the book for</param>
        /// <param name="limit">Limit to book to the best x bids/asks</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        public async Task<WebCallResult<KrakenOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("count", limit);
            var result = await Execute<Dictionary<string, KrakenOrderBook>>(GetUri("/0/public/Depth"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if(!result)
                return new WebCallResult<KrakenOrderBook>(result.ResponseStatusCode, result.ResponseHeaders, null, result.Error);
            return new WebCallResult<KrakenOrderBook>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.First().Value, result.Error);
        }

        /// <summary>
        /// Get a list of recent trades for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get trades for</param>
        /// <param name="since">Return trades since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Recent trades</returns>
        public WebCallResult<KrakenTradesResult> GetRecentTrades(string symbol, DateTime? since = null, CancellationToken ct = default) => GetRecentTradesAsync(symbol, since, ct).Result;
        /// <summary>
        /// Get a list of recent trades for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get trades for</param>
        /// <param name="since">Return trades since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Recent trades</returns>
        public async Task<WebCallResult<KrakenTradesResult>> GetRecentTradesAsync(string symbol, DateTime? since = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("since", since.HasValue ? JsonConvert.SerializeObject(since, new TimestampSecondsConverter()) : null);
            return await Execute<KrakenTradesResult>(GetUri("/0/public/Trades"), HttpMethod.Get, ct, parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get spread data for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get spread data for</param>
        /// <param name="since">Return spread data since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Spread data</returns>
        public WebCallResult<KrakenSpreadsResult> GetRecentSpread(string symbol, DateTime? since = null, CancellationToken ct = default) => GetRecentSpreadAsync(symbol, since, ct).Result;
        /// <summary>
        /// Get spread data for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get spread data for</param>
        /// <param name="since">Return spread data since a specific time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Spread data</returns>
        public async Task<WebCallResult<KrakenSpreadsResult>> GetRecentSpreadAsync(string symbol, DateTime? since = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("since", since.HasValue ? JsonConvert.SerializeObject(since, new TimestampSecondsConverter()) : null);
            return await Execute<KrakenSpreadsResult>(GetUri("/0/public/Spread"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get balances
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with balances for assets</returns>
        public WebCallResult<Dictionary<string, decimal>> GetBalances(CancellationToken ct = default) => GetBalancesAsync(ct).Result;
        /// <summary>
        /// Get balances
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with balances for assets</returns>
        public async Task<WebCallResult<Dictionary<string, decimal>>> GetBalancesAsync(CancellationToken ct = default)
        {
            return await Execute<Dictionary<string, decimal>>(GetUri("/0/private/Balance"), HttpMethod.Post, ct, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get trade balance
        /// </summary>
        /// <param name="baseAsset">Base asset to get trade balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade balance data</returns>
        public WebCallResult<KrakenTradeBalance> GetTradeBalance(string? baseAsset = null, CancellationToken ct = default) => GetTradeBalanceAsync(baseAsset, ct).Result;
        /// <summary>
        /// Get trade balance
        /// </summary>
        /// <param name="baseAsset">Base asset to get trade balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade balance data</returns>
        public async Task<WebCallResult<KrakenTradeBalance>> GetTradeBalanceAsync(string? baseAsset = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("aclass", "currency");
            parameters.AddOptionalParameter("asset", baseAsset);
            return await Execute<KrakenTradeBalance>(GetUri("/0/private/TradeBalance"), HttpMethod.Post, ct, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of open orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        public WebCallResult<OpenOrdersPage> GetOpenOrders(string? clientOrderId = null, CancellationToken ct = default) => GetOpenOrdersAsync(clientOrderId, ct).Result;
        /// <summary>
        /// Get a list of open orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        public async Task<WebCallResult<OpenOrdersPage>> GetOpenOrdersAsync(string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            return await Execute<OpenOrdersPage>(GetUri("/0/private/OpenOrders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of closed orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Closed orders page</returns>
        public WebCallResult<KrakenClosedOrdersPage> GetClosedOrders(string? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, CancellationToken ct = default) => 
            GetClosedOrdersAsync(clientOrderId, startTime, endTime, resultOffset, ct).Result;
        /// <summary>
        /// Get a list of closed orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Closed orders page</returns>
        public async Task<WebCallResult<KrakenClosedOrdersPage>> GetClosedOrdersAsync(string? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("start", startTime.HasValue ? JsonConvert.SerializeObject(startTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("end", endTime.HasValue ? JsonConvert.SerializeObject(endTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("ofs", resultOffset);
            return await Execute<KrakenClosedOrdersPage>(GetUri("/0/private/ClosedOrders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get info on specific orders
        /// </summary>
        /// <param name="clientOrderId">Get orders by clientOrderId</param>
        /// <param name="orderIds">Get orders by their order ids</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with order info</returns>
        public WebCallResult<Dictionary<string, KrakenOrder>> GetOrders(string? clientOrderId = null, CancellationToken ct = default, params string[] orderIds) => GetOrdersAsync(clientOrderId, ct, orderIds).Result;
        /// <summary>
        /// Get info on specific orders
        /// </summary>
        /// <param name="clientOrderId">Get orders by clientOrderId</param>
        /// <param name="orderIds">Get orders by their order ids</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with order info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrdersAsync(string? clientOrderId = null, CancellationToken ct = default, params string[] orderIds)
        {
            if((string.IsNullOrEmpty(clientOrderId) && !orderIds.Any()) || (!string.IsNullOrEmpty(clientOrderId) && orderIds.Any()))
                throw new ArgumentException("Either clientOrderId or ordersIds should be provided");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("txid", orderIds.Any() ? string.Join(",", orderIds): null);
            return await Execute<Dictionary<string, KrakenOrder>>(GetUri("/0/private/QueryOrders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get trade history
        /// </summary>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade history page</returns>
        public WebCallResult<KrakenUserTradesPage> GetTradeHistory(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, CancellationToken ct = default) => 
            GetTradeHistoryAsync(startTime, endTime, resultOffset, ct).Result;
        /// <summary>
        /// Get trade history
        /// </summary>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade history page</returns>
        public async Task<WebCallResult<KrakenUserTradesPage>> GetTradeHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("start", startTime.HasValue ? JsonConvert.SerializeObject(startTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("end", endTime.HasValue ? JsonConvert.SerializeObject(endTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("ofs", resultOffset);
            return await Execute<KrakenUserTradesPage>(GetUri("/0/private/TradesHistory"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get info on specific trades
        /// </summary>
        /// <param name="tradeIds">The trades to get info on</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with trade info</returns>
        public WebCallResult<Dictionary<string, KrakenUserTrade>> GetTrades(CancellationToken ct = default, params string[] tradeIds) => GetTradesAsync(ct, tradeIds).Result;
        /// <summary>
        /// Get info on specific trades
        /// </summary>
        /// <param name="tradeIds">The trades to get info on</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with trade info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetTradesAsync(CancellationToken ct = default, params string[] tradeIds)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("txid", tradeIds.Any() ? string.Join(",", tradeIds) : null);
            return await Execute<Dictionary<string, KrakenUserTrade>>(GetUri("/0/private/QueryTrades"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of open positions
        /// </summary>
        /// <param name="transactionIds">Filter by transaction ids</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with position info</returns>
        public WebCallResult<Dictionary<string, KrakenPosition>> GetOpenPositions(CancellationToken ct = default, params string[] transactionIds) => GetOpenPositionsAsync(ct, transactionIds).Result;
        /// <summary>
        /// Get a list of open positions
        /// </summary>
        /// <param name="transactionIds">Filter by transaction ids</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with position info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(CancellationToken ct = default, params string[] transactionIds)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("docalcs", true);
            parameters.AddOptionalParameter("txid", transactionIds.Any() ? string.Join(",", transactionIds) : null);
            return await Execute<Dictionary<string, KrakenPosition>>(GetUri("/0/private/OpenPositions"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get ledger entries info
        /// </summary>
        /// <param name="assets">Filter list by asset names</param>
        /// <param name="entryTypes">Filter list by entry types</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Ledger entries page</returns>
        public WebCallResult<KrakenLedgerPage> GetLedgerInfo(IEnumerable<string>? assets = null, IEnumerable<LedgerEntryType>? entryTypes = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, CancellationToken ct = default) => 
            GetLedgerInfoAsync(assets, entryTypes, startTime, endTime, resultOffset, ct).Result;
        /// <summary>
        /// Get ledger entries info
        /// </summary>
        /// <param name="assets">Filter list by asset names</param>
        /// <param name="entryTypes">Filter list by entry types</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Ledger entries page</returns>
        public async Task<WebCallResult<KrakenLedgerPage>> GetLedgerInfoAsync(IEnumerable<string>? assets = null, IEnumerable<LedgerEntryType>? entryTypes = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", assets != null ? string.Join(",", assets) : null);
            parameters.AddOptionalParameter("type", entryTypes != null ? string.Join(",", entryTypes.Select(e => JsonConvert.SerializeObject(e, new LedgerEntryTypeConverter(false)))) : null);
            parameters.AddOptionalParameter("start", startTime.HasValue ? JsonConvert.SerializeObject(startTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("end", endTime.HasValue ? JsonConvert.SerializeObject(endTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("ofs", resultOffset);
            return await Execute<KrakenLedgerPage>(GetUri("/0/private/Ledgers"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get info on specific ledger entries
        /// </summary>
        /// <param name="ledgerIds">The ids to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ledger entry info</returns>
        public WebCallResult<Dictionary<string, KrakenLedgerEntry>> GetLedgersEntry(CancellationToken ct = default, params string[] ledgerIds) => GetLedgersEntryAsync(ct, ledgerIds).Result;
        /// <summary>
        /// Get info on specific ledger entries
        /// </summary>
        /// <param name="ledgerIds">The ids to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ledger entry info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(CancellationToken ct = default, params string[] ledgerIds)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("id", ledgerIds.Any() ? string.Join(",", ledgerIds) : null);
            return await Execute<Dictionary<string, KrakenLedgerEntry>>(GetUri("/0/private/QueryLedgers"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get trade volume
        /// </summary>
        /// <param name="symbols">Symbols to get data for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade fee info</returns>
        public WebCallResult<KrakenTradeVolume> GetTradeVolume(CancellationToken ct = default, params string[] symbols) => GetTradeVolumeAsync(ct, symbols).Result;
        /// <summary>
        /// Get trade volume
        /// </summary>
        /// <param name="symbols">Symbols to get data for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade fee info</returns>
        public async Task<WebCallResult<KrakenTradeVolume>> GetTradeVolumeAsync(CancellationToken ct = default, params string[] symbols)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("fee-info", true);
            parameters.AddOptionalParameter("pair", symbols.Any() ? string.Join(",", symbols) : null);
            return await Execute<KrakenTradeVolume>(GetUri("/0/private/TradeVolume"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get deposit methods
        /// </summary>
        /// <param name="asset">Asset to get methods for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Array with methods for deposit</returns>
        public WebCallResult<IEnumerable<KrakenDepositMethod>> GetDepositMethods(string asset, CancellationToken ct = default) => GetDepositMethodsAsync(asset, ct).Result;
        /// <summary>
        /// Get deposit methods
        /// </summary>
        /// <param name="asset">Asset to get methods for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Array with methods for deposit</returns>
        public async Task<WebCallResult<IEnumerable<KrakenDepositMethod>>> GetDepositMethodsAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new Dictionary<string, object>()
            {
                {"asset", asset}
            };
            return await Execute< IEnumerable<KrakenDepositMethod>>(GetUri("/0/private/DepositMethods"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get deposit addresses for an asset
        /// </summary>
        /// <param name="asset">The asset to get the deposit address for</param>
        /// <param name="depositMethod">The method of deposit</param>
        /// <param name="generateNew">Whether to generate a new address</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<KrakenDepositAddress>> GetDepositAddresses(string asset, string depositMethod, bool generateNew = false, CancellationToken ct = default) => 
            GetDepositAddressesAsync(asset, depositMethod, generateNew, ct).Result;
        /// <summary>
        /// Get deposit addresses for an asset
        /// </summary>
        /// <param name="asset">The asset to get the deposit address for</param>
        /// <param name="depositMethod">The method of deposit</param>
        /// <param name="generateNew">Whether to generate a new address</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<KrakenDepositAddress>>> GetDepositAddressesAsync(string asset, string depositMethod, bool generateNew = false, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            depositMethod.ValidateNotNull(nameof(depositMethod));
            var parameters = new Dictionary<string, object>()
            {
                {"asset", asset},
                {"method", depositMethod},
            };

            if(generateNew)
                // If False is send it will still generate new, so only add it when it's true
                parameters.Add("new", true);

            return await Execute<IEnumerable<KrakenDepositAddress>>(GetUri("/0/private/DepositAddresses"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get status deposits for an asset
        /// </summary>
        /// <param name="asset">Asset to get deposit info for</param>
        /// <param name="depositMethod">The deposit method</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit status list</returns>
        public WebCallResult<IEnumerable<KrakenDepositStatus>> GetDepositStatus(string asset, string depositMethod, CancellationToken ct = default) => GetDepositStatusAsync(asset, depositMethod, ct).Result;
        /// <summary>
        /// Get status deposits for an asset
        /// </summary>
        /// <param name="asset">Asset to get deposit info for</param>
        /// <param name="depositMethod">The deposit method</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit status list</returns>
        public async Task<WebCallResult<IEnumerable<KrakenDepositStatus>>> GetDepositStatusAsync(string asset, string depositMethod, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            depositMethod.ValidateNotNull(nameof(depositMethod));
            var parameters = new Dictionary<string, object>()
            {
                {"asset", asset},
                {"method", depositMethod},
            };

            return await Execute<IEnumerable<KrakenDepositStatus>>(GetUri("/0/private/DepositStatus"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

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
        /// <param name="ct">Cancellation token</param>
        /// <returns>Placed order info</returns>
        public WebCallResult<KrakenPlacedOrder> PlaceOrder(
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
            CancellationToken ct = default) => PlaceOrderAsync(symbol, side, type, quantity, clientOrderId, price, secondaryPrice, leverage, startTime, expireTime, validateOnly, ct).Result;
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
        /// <param name="ct">Cancellation token</param>
        /// <returns>Placed order info</returns>
        public async Task<WebCallResult<KrakenPlacedOrder>> PlaceOrderAsync(
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
            CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                { "pair", symbol },
                { "type", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "ordertype", JsonConvert.SerializeObject(type, new OrderTypeConverter(false)) },
                { "volume", quantity },
            };
            parameters.AddOptionalParameter("price", price);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("price2", secondaryPrice);
            parameters.AddOptionalParameter("leverage", leverage);
            parameters.AddOptionalParameter("starttm", startTime.HasValue ? JsonConvert.SerializeObject(startTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("expiretm", expireTime.HasValue ? JsonConvert.SerializeObject(expireTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("validate", validateOnly);
            return await Execute<KrakenPlacedOrder>(GetUri("/0/private/AddOrder"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">The id of the order to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Cancel result</returns>
        public WebCallResult<KrakenCancelResult> CancelOrder(string orderId, CancellationToken ct = default) => CancelOrderAsync(orderId, ct).Result;
        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">The id of the order to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Cancel result</returns>
        public async Task<WebCallResult<KrakenCancelResult>> CancelOrderAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            var parameters = new Dictionary<string, object>()
            {
                {"txid", orderId}
            };
            return await Execute<KrakenCancelResult>(GetUri("/0/private/CancelOrder"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion
        /// <inheritdoc />
        protected override void WriteParamBody(IRequest request, Dictionary<string, object> parameters, string contentType)
        {
            var stringData = string.Join("&", parameters.OrderBy(p => p.Key != "nonce").Select(p => $"{p.Key}={p.Value}"));
            request.SetContent(stringData, contentType);
        }

        private Uri GetUri(string endpoint)
        {
            return new Uri(BaseAddress + endpoint);
        }

        private async Task<WebCallResult<T>> Execute<T>(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false)
        {
            var result = await SendRequest<KrakenResult<T>>(url, method, ct, parameters, signed).ConfigureAwait(false);
            if (!result)
                return new WebCallResult<T>(result.ResponseStatusCode, result.ResponseHeaders, default, result.Error);

            if (result.Data.Error.Any())
                return new WebCallResult<T>(result.ResponseStatusCode, result.ResponseHeaders, default, new ServerError(string.Join(", ", result.Data.Error)));

            return new WebCallResult<T>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Result, null);
        }
    }
}
