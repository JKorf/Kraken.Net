using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net;
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
            Configure(options);
        }
        #endregion

        #region methods

        /// <summary>
        /// Get the server time
        /// </summary>
        /// <returns>Server time</returns>
        public CallResult<DateTime> GetServerTime() => GetServerTimeAsync().Result;
        /// <summary>
        /// Get the server time
        /// </summary>
        /// <returns>Server time</returns>
        public async Task<CallResult<DateTime>> GetServerTimeAsync()
        {
            var result = await Execute<KrakenServerTime>(GetUri("/0/public/Time")).ConfigureAwait(false);
            if (!result.Success)
                return WebCallResult<DateTime>.CreateErrorResult(result.Error);
            return new CallResult<DateTime>(result.Data.UnixTime, null);
        }

        /// <summary>
        /// Get a list of assets and info about them
        /// </summary>
        /// <param name="assets">Filter list for specific assets</param>
        /// <returns>Dictionary of asset info</returns>
        public WebCallResult<Dictionary<string, KrakenAssetInfo>> GetAssets(params string[] assets) => GetAssetsAsync(assets).Result;
        /// <summary>
        /// Get a list of assets and info about them
        /// </summary>
        /// <param name="assets">Filter list for specific assets</param>
        /// <returns>Dictionary of asset info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenAssetInfo>>> GetAssetsAsync(params string[] assets)
        {
            var parameters = new Dictionary<string, object>();
            if(assets.Any())
                parameters.AddOptionalParameter("asset", string.Join(",", assets));

            return await Execute<Dictionary<string, KrakenAssetInfo>>(GetUri("/0/public/Assets"), parameters:parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of markets and info about them
        /// </summary>
        /// <param name="markets">Filter list for specific markets</param>
        /// <returns>Dictionary of market info</returns>
        public WebCallResult<Dictionary<string, KrakenMarket>> GetMarkets(params string[] markets) => GetMarketsAsync(markets).Result;
        /// <summary>
        /// Get a list of markets and info about them
        /// </summary>
        /// <param name="markets">Filter list for specific markets</param>
        /// <returns>Dictionary of market info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenMarket>>> GetMarketsAsync(params string[] markets)
        {
            var parameters = new Dictionary<string, object>();
            if (markets.Any())
                parameters.AddOptionalParameter("pair", string.Join(",", markets));

            return await Execute<Dictionary<string, KrakenMarket>>(GetUri("/0/public/AssetPairs"), parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get tickers for markets
        /// </summary>
        /// <param name="markets">Markets to get tickers for</param>
        /// <returns>Dictionary with ticker info</returns>
        public WebCallResult<Dictionary<string, KrakenRestTick>> GetTickers(params string[] markets) => GetTickersAsync(markets).Result;
        /// <summary>
        /// Get tickers for markets
        /// </summary>
        /// <param name="markets">Markets to get tickers for</param>
        /// <returns>Dictionary with ticker info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(params string[] markets)
        {
            if (!markets.Any())
                return WebCallResult<Dictionary<string, KrakenRestTick>>.CreateErrorResult(new ArgumentError("Specify markets to get tickers for"));

            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("pair", string.Join(",", markets));

            return await Execute<Dictionary<string, KrakenRestTick>>(GetUri("/0/public/Ticker"), parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets kline data for a market
        /// </summary>
        /// <param name="market">The market to get data for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="since">Return klines since a secific time</param>
        /// <returns>Kline data</returns>
        public WebCallResult<KrakenKlinesResult> GetKlines(string market, KlineInterval interval, DateTime? since = null) => GetKlinesAsync(market, interval, since).Result;
        /// <summary>
        /// Gets kline data for a market
        /// </summary>
        /// <param name="market">The market to get data for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="since">Return klines since a secific time</param>
        /// <returns>Kline data</returns>
        public async Task<WebCallResult<KrakenKlinesResult>> GetKlinesAsync(string market, KlineInterval interval, DateTime? since = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"pair", market},
                {"interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}
            };
            parameters.AddOptionalParameter("since", since.HasValue ? JsonConvert.SerializeObject(since, new TimestampSecondsConverter()): null);
            return await Execute<KrakenKlinesResult>(GetUri("/0/public/OHLC"), parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the order book for a market
        /// </summary>
        /// <param name="market">Market to get the book for</param>
        /// <param name="limit">Limit to book to the best x bids/asks</param>
        /// <returns>Order book for the market</returns>
        public WebCallResult<KrakenOrderBook> GetOrderBook(string market, int? limit = null) => GetOrderBookAsync(market, limit).Result;
        /// <summary>
        /// Get the order book for a market
        /// </summary>
        /// <param name="market">Market to get the book for</param>
        /// <param name="limit">Limit to book to the best x bids/asks</param>
        /// <returns>Order book for the market</returns>
        public async Task<WebCallResult<KrakenOrderBook>> GetOrderBookAsync(string market, int? limit = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"pair", market},
            };
            parameters.AddOptionalParameter("count", limit);
            var result = await Execute<Dictionary<string, KrakenOrderBook>>(GetUri("/0/public/Depth"), parameters: parameters).ConfigureAwait(false);
            if(!result.Success)
                return new WebCallResult<KrakenOrderBook>(result.ResponseStatusCode, result.ResponseHeaders, null, result.Error);
            return new WebCallResult<KrakenOrderBook>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.First().Value, result.Error);
        }

        /// <summary>
        /// Get a list of recent trades for a market
        /// </summary>
        /// <param name="market">Market to get trades for</param>
        /// <param name="since">Return trades since a specific time</param>
        /// <returns>Recent trades</returns>
        public WebCallResult<KrakenTradesResult> GetRecentTrades(string market, DateTime? since = null) => GetRecentTradesAsync(market, since).Result;
        /// <summary>
        /// Get a list of recent trades for a market
        /// </summary>
        /// <param name="market">Market to get trades for</param>
        /// <param name="since">Return trades since a specific time</param>
        /// <returns>Recent trades</returns>
        public async Task<WebCallResult<KrakenTradesResult>> GetRecentTradesAsync(string market, DateTime? since = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"pair", market},
            };
            parameters.AddOptionalParameter("since", since.HasValue ? JsonConvert.SerializeObject(since, new TimestampSecondsConverter()) : null);
            return await Execute<KrakenTradesResult>(GetUri("/0/public/Trades"), parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get spread data for a market
        /// </summary>
        /// <param name="market">Market to get spread data for</param>
        /// <param name="since">Return spread data since a specific time</param>
        /// <returns>Spread data</returns>
        public WebCallResult<KrakenSpreadsResult> GetRecentSpread(string market, DateTime? since = null) => GetRecentSpreadAsync(market, since).Result;
        /// <summary>
        /// Get spread data for a market
        /// </summary>
        /// <param name="market">Market to get spread data for</param>
        /// <param name="since">Return spread data since a specific time</param>
        /// <returns>Spread data</returns>
        public async Task<WebCallResult<KrakenSpreadsResult>> GetRecentSpreadAsync(string market, DateTime? since = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"pair", market},
            };
            parameters.AddOptionalParameter("since", since.HasValue ? JsonConvert.SerializeObject(since, new TimestampSecondsConverter()) : null);
            return await Execute<KrakenSpreadsResult>(GetUri("/0/public/Spread"), parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get balances
        /// </summary>
        /// <returns>Dictionary with balances for assets</returns>
        public WebCallResult<Dictionary<string, decimal>> GetAccountBalance() => GetAccountBalanceAsync().Result;
        /// <summary>
        /// Get balances
        /// </summary>
        /// <returns>Dictionary with balances for assets</returns>
        public async Task<WebCallResult<Dictionary<string, decimal>>> GetAccountBalanceAsync()
        {
            return await Execute<Dictionary<string, decimal>>(GetUri("/0/private/Balance"), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get trade balance
        /// </summary>
        /// <param name="baseAsset">Base asset to get trade balance for</param>
        /// <returns>Trade balance data</returns>
        public WebCallResult<KrakenTradeBalance> GetTradeBalance(string baseAsset = null) => GetTradeBalanceAsync(baseAsset).Result;
        /// <summary>
        /// Get trade balance
        /// </summary>
        /// <param name="baseAsset">Base asset to get trade balance for</param>
        /// <returns>Trade balance data</returns>
        public async Task<WebCallResult<KrakenTradeBalance>> GetTradeBalanceAsync(string baseAsset = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("aclass", "currency");
            parameters.AddOptionalParameter("asset", baseAsset);
            return await Execute<KrakenTradeBalance>(GetUri("/0/private/TradeBalance"), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of open orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <returns>List of open orders</returns>
        public WebCallResult<OpenOrdersPage> GetOpenOrders(string clientOrderId = null) => GetOpenOrdersAsync(clientOrderId).Result;
        /// <summary>
        /// Get a list of open orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <returns>List of open orders</returns>
        public async Task<WebCallResult<OpenOrdersPage>> GetOpenOrdersAsync(string clientOrderId = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            return await Execute<OpenOrdersPage>(GetUri("/0/private/OpenOrders"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of closed orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <returns>Closed orders page</returns>
        public WebCallResult<KrakenClosedOrdersPage> GetClosedOrders(string clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null) => GetClosedOrdersAsync(clientOrderId, startTime, endTime, resultOffset).Result;
        /// <summary>
        /// Get a list of closed orders
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <returns>Closed orders page</returns>
        public async Task<WebCallResult<KrakenClosedOrdersPage>> GetClosedOrdersAsync(string clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("start", startTime.HasValue ? JsonConvert.SerializeObject(startTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("end", endTime.HasValue ? JsonConvert.SerializeObject(endTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("ofs", resultOffset);
            return await Execute<KrakenClosedOrdersPage>(GetUri("/0/private/ClosedOrders"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get info on specific orders
        /// </summary>
        /// <param name="clientOrderId">Get orders by clientOrderId</param>
        /// <param name="orderIds">Get orders by their order ids</param>
        /// <returns>Dictionary with order info</returns>
        public WebCallResult<Dictionary<string, KrakenOrder>> GetOrders(string clientOrderId = null, params string[] orderIds) => GetOrdersAsync(clientOrderId, orderIds).Result;
        /// <summary>
        /// Get info on specific orders
        /// </summary>
        /// <param name="clientOrderId">Get orders by clientOrderId</param>
        /// <param name="orderIds">Get orders by their order ids</param>
        /// <returns>Dictionary with order info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrdersAsync(string clientOrderId = null, params string[] orderIds)
        {
            if((string.IsNullOrEmpty(clientOrderId) && !orderIds.Any()) || (!string.IsNullOrEmpty(clientOrderId) && orderIds.Any()))
                return WebCallResult<Dictionary<string, KrakenOrder>>.CreateErrorResult(new ArgumentError("Provide either clientOrderId or orderIds"));

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("txid", orderIds.Any() ? string.Join(",", orderIds): null);
            return await Execute<Dictionary<string, KrakenOrder>>(GetUri("/0/private/QueryOrders"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get trade history
        /// </summary>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <returns>Trade history page</returns>
        public WebCallResult<KrakenUserTradesPage> GetTradeHistory(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null) => GetTradeHistoryAsync(startTime, endTime, resultOffset).Result;
        /// <summary>
        /// Get trade history
        /// </summary>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <returns>Trade history page</returns>
        public async Task<WebCallResult<KrakenUserTradesPage>> GetTradeHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("start", startTime.HasValue ? JsonConvert.SerializeObject(startTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("end", endTime.HasValue ? JsonConvert.SerializeObject(endTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("ofs", resultOffset);
            return await Execute<KrakenUserTradesPage>(GetUri("/0/private/TradesHistory"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get info on specific trades
        /// </summary>
        /// <param name="tradeIds">The trades to get info on</param>
        /// <returns>Dictionary with trade info</returns>
        public WebCallResult<Dictionary<string, KrakenUserTrade>> GetTrades(params string[] tradeIds) => GetTradesAsync(tradeIds).Result;
        /// <summary>
        /// Get info on specific trades
        /// </summary>
        /// <param name="tradeIds">The trades to get info on</param>
        /// <returns>Dictionary with trade info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetTradesAsync(params string[] tradeIds)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("txid", tradeIds.Any() ? string.Join(",", tradeIds) : null);
            return await Execute<Dictionary<string, KrakenUserTrade>>(GetUri("/0/private/QueryTrades"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a list of open positions
        /// </summary>
        /// <param name="transactionIds">Filter by transaction ids</param>
        /// <returns>Dictionary with position info</returns>
        public WebCallResult<Dictionary<string, KrakenPosition>> GetOpenPositions(params string[] transactionIds) => GetOpenPositionsAsync(transactionIds).Result;
        /// <summary>
        /// Get a list of open positions
        /// </summary>
        /// <param name="transactionIds">Filter by transaction ids</param>
        /// <returns>Dictionary with position info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(params string[] transactionIds)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("docalcs", true);
            parameters.AddOptionalParameter("txid", transactionIds.Any() ? string.Join(",", transactionIds) : null);
            return await Execute<Dictionary<string, KrakenPosition>>(GetUri("/0/private/OpenPositions"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get ledger entries info
        /// </summary>
        /// <param name="assets">Filter list by asset names</param>
        /// <param name="entryTypes">Filter list by entry types</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <returns>Ledger entries page</returns>
        public WebCallResult<KrakenLedgerPage> GetLedgerInfo(string[] assets = null, LedgerEntryType[] entryTypes = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null) => GetLedgerInfoAsync(assets, entryTypes, startTime, endTime, resultOffset).Result;
        /// <summary>
        /// Get ledger entries info
        /// </summary>
        /// <param name="assets">Filter list by asset names</param>
        /// <param name="entryTypes">Filter list by entry types</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <returns>Ledger entries page</returns>
        public async Task<WebCallResult<KrakenLedgerPage>> GetLedgerInfoAsync(string[] assets = null, LedgerEntryType[] entryTypes = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", assets != null ? string.Join(",", assets) : null);
            parameters.AddOptionalParameter("type", entryTypes != null ? string.Join(",", entryTypes.Select(e => JsonConvert.SerializeObject(e, new LedgerEntryTypeConverter()))) : null);
            parameters.AddOptionalParameter("start", startTime.HasValue ? JsonConvert.SerializeObject(startTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("end", endTime.HasValue ? JsonConvert.SerializeObject(endTime.Value, new TimestampSecondsConverter()) : null);
            parameters.AddOptionalParameter("ofs", resultOffset);
            return await Execute<KrakenLedgerPage>(GetUri("/0/private/Ledgers"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get info on specific ledger entries
        /// </summary>
        /// <param name="ledgerIds">The ids to get info for</param>
        /// <returns>Dictionary with ledger entry info</returns>
        public WebCallResult<Dictionary<string, KrakenLedgerEntry>> GetLedgersEntry(params string[] ledgerIds) => GetLedgersEntryAsync(ledgerIds).Result;
        /// <summary>
        /// Get info on specific ledger entries
        /// </summary>
        /// <param name="ledgerIds">The ids to get info for</param>
        /// <returns>Dictionary with ledger entry info</returns>
        public async Task<WebCallResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(params string[] ledgerIds)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("id", ledgerIds.Any() ? string.Join(",", ledgerIds) : null);
            return await Execute<Dictionary<string, KrakenLedgerEntry>>(GetUri("/0/private/QueryLedgers"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get trade volume
        /// </summary>
        /// <param name="markets">Markets to get data for</param>
        /// <returns>Trade fee info</returns>
        public WebCallResult<KrakenTradeVolume> GetTradeVolume(params string[] markets) => GetTradeVolumeAsync(markets).Result;
        /// <summary>
        /// Get trade volume
        /// </summary>
        /// <param name="markets">Markets to get data for</param>
        /// <returns>Trade fee info</returns>
        public async Task<WebCallResult<KrakenTradeVolume>> GetTradeVolumeAsync(params string[] markets)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("fee-info", true);
            parameters.AddOptionalParameter("pair", markets.Any() ? string.Join(",", markets) : null);
            return await Execute<KrakenTradeVolume>(GetUri("/0/private/TradeVolume"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get deposit methods
        /// </summary>
        /// <param name="asset">Asset to get methods for</param>
        /// <returns>Array with methods for deposit</returns>
        public WebCallResult<KrakenDepositMethod[]> GetDepositMethods(string asset) => GetDepositMethodsAsync(asset).Result;
        /// <summary>
        /// Get deposit methods
        /// </summary>
        /// <param name="asset">Asset to get methods for</param>
        /// <returns>Array with methods for deposit</returns>
        public async Task<WebCallResult<KrakenDepositMethod[]>> GetDepositMethodsAsync(string asset)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"asset", asset}
            };
            return await Execute<KrakenDepositMethod[]>(GetUri("/0/private/DepositMethods"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get deposit addresses for an asset
        /// </summary>
        /// <param name="asset">The asset to get the deposit address for</param>
        /// <param name="depositMethod">The method of deposit</param>
        /// <param name="generateNew">Whether to generate a new address</param>
        /// <returns></returns>
        public WebCallResult<KrakenDepositAddress[]> GetDepositAddresses(string asset, string depositMethod, bool generateNew = false) => GetDepositAddressesAsync(asset, depositMethod, generateNew).Result;
        /// <summary>
        /// Get deposit addresses for an asset
        /// </summary>
        /// <param name="asset">The asset to get the deposit address for</param>
        /// <param name="depositMethod">The method of deposit</param>
        /// <param name="generateNew">Whether to generate a new address</param>
        /// <returns></returns>
        public async Task<WebCallResult<KrakenDepositAddress[]>> GetDepositAddressesAsync(string asset, string depositMethod, bool generateNew = false)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"asset", asset},
                {"method", depositMethod},
            };

            if(generateNew)
                // If False is send it will still generate new, so only add it when it's true
                parameters.Add("new", generateNew);

            return await Execute<KrakenDepositAddress[]>(GetUri("/0/private/DepositAddresses"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get status deposits for an asset
        /// </summary>
        /// <param name="asset">Asset to get deposit info for</param>
        /// <param name="depositMethod">The deposit method</param>
        /// <returns>Deposit status list</returns>
        public WebCallResult<KrakenDepositStatus[]> GetDepositStatus(string asset, string depositMethod) => GetDepositStatusAsync(asset, depositMethod).Result;
        /// <summary>
        /// Get status deposits for an asset
        /// </summary>
        /// <param name="asset">Asset to get deposit info for</param>
        /// <param name="depositMethod">The deposit method</param>
        /// <returns>Deposit status list</returns>
        public async Task<WebCallResult<KrakenDepositStatus[]>> GetDepositStatusAsync(string asset, string depositMethod)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"asset", asset},
                {"method", depositMethod},
            };

            return await Execute<KrakenDepositStatus[]>(GetUri("/0/private/DepositStatus"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Place a new order
        /// </summary>
        /// <param name="market">The market the order is on</param>
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
        /// <returns>Placed order info</returns>
        public WebCallResult<KrakenPlacedOrder> PlaceOrder(
            string market, 
            OrderSide side, 
            OrderType type, 
            decimal quantity, 
            uint? clientOrderId = null,
            decimal? price = null, 
            decimal? secondaryPrice = null,
            decimal? leverage = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            bool? validateOnly = null) => PlaceOrderAsync(market, side, type, quantity, clientOrderId, price, secondaryPrice, leverage, startTime, expireTime, validateOnly).Result;
        /// <summary>
        /// Place a new order
        /// </summary>
        /// <param name="market">The market the order is on</param>
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
        /// <returns>Placed order info</returns>
        public async Task<WebCallResult<KrakenPlacedOrder>> PlaceOrderAsync(
            string market,
            OrderSide side,
            OrderType type,
            decimal quantity,
            uint? clientOrderId = null,
            decimal? price = null,
            decimal? secondaryPrice = null,
            decimal? leverage = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            bool? validateOnly = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "pair", market },
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
            return await Execute<KrakenPlacedOrder>(GetUri("/0/private/AddOrder"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">The id of the order to cancel</param>
        /// <returns>Cancel result</returns>
        public WebCallResult<KrakenCancelResult> CancelOrder(string orderId) => CancelOrderAsync(orderId).Result;
        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">The id of the order to cancel</param>
        /// <returns>Cancel result</returns>
        public async Task<WebCallResult<KrakenCancelResult>> CancelOrderAsync(string orderId)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"txid", orderId}
            };
            return await Execute<KrakenCancelResult>(GetUri("/0/private/CancelOrder"), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        #endregion
        /// <inheritdoc />
        protected override void WriteParamBody(IRequest request, Dictionary<string, object> parameters)
        {
            var stringData = string.Join("&", parameters.OrderBy(p => p.Key != "nonce").Select(p => $"{p.Key}={p.Value}"));
            WriteParamBody(request, stringData);
        }

        private Uri GetUri(string endpoint)
        {
            return new Uri(BaseAddress + endpoint);
        }

        private async Task<WebCallResult<T>> Execute<T>(Uri url, string method = Constants.GetMethod, Dictionary<string, object> parameters = null, bool signed = false)
        {
            var result = await ExecuteRequest<KrakenResult<T>>(url, method, parameters, signed).ConfigureAwait(false);
            if (!result.Success)
                return new WebCallResult<T>(result.ResponseStatusCode, result.ResponseHeaders, default, result.Error);

            if (result.Data.Error.Any())
                return new WebCallResult<T>(result.ResponseStatusCode, result.ResponseHeaders, default, new ServerError(string.Join(", ", result.Data.Error)));

            return new WebCallResult<T>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Result, null);
        }
    }
}
