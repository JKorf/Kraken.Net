using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.CommonObjects;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class KrakenRestClientSpotApiTrading : IKrakenRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly KrakenRestClientSpotApi _baseClient;

        internal KrakenRestClientSpotApiTrading(KrakenRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<OpenOrdersPage>> GetOpenOrdersAsync(uint? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/OpenOrders", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<OpenOrdersPage>(request, parameters, ct).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data.Open)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenClosedOrdersPage>> GetClosedOrdersAsync(uint? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.AddOptionalParameter("ofs", resultOffset);
            parameters.AddOptionalParameter("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/ClosedOrders", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenClosedOrdersPage>(request, parameters, ct).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data.Closed)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrderAsync(string? orderId = null, uint? clientOrderId = null, bool? consolidateTaker = null, bool? trades = null, string? twoFactorPassword = null, CancellationToken ct = default)
            => GetOrdersAsync(orderId == null ? null : new[] { orderId }, clientOrderId, consolidateTaker, trades, twoFactorPassword, ct);

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrdersAsync(IEnumerable<string>? orderIds = null, uint? clientOrderId = null, bool? consolidateTaker = null, bool? trades = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("txid", orderIds?.Any() == true ? string.Join(",", orderIds) : null);
            parameters.AddOptionalParameter("consolidate_taker", consolidateTaker);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            parameters.AddOptionalParameter("trades", trades);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/QueryOrders", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenOrder>>(request, parameters, ct).ConfigureAwait(false);
            
            if (result)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenUserTradesPage>> GetUserTradesAsync(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, bool? consolidateTaker = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.AddOptionalParameter("ofs", resultOffset);
            parameters.AddOptionalParameter("consolidate_taker", consolidateTaker);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/TradesHistory", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenUserTradesPage>(request, parameters, ct).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data.Trades)
                    item.Value.Id = item.Key;
            }

            return result;
        }

        #endregion

        #region Get User Trade Details

        /// <inheritdoc />
        public Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(string tradeId, string? twoFactorPassword = null, CancellationToken ct = default)
            => GetUserTradeDetailsAsync(new[] { tradeId }, twoFactorPassword, ct);


        #endregion

        #region Get User Trade Details

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(IEnumerable<string> tradeIds, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("txid", tradeIds?.Any() == true ? string.Join(",", tradeIds) : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
           
            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/QueryTrades", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenUserTrade>>(request, parameters, ct).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenBatchOrderResult>> PlaceMultipleOrdersAsync(string symbol, IEnumerable<KrakenOrderRequest> orders, DateTime? deadline = null, bool? validateOnly = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "pair", symbol },
                { "trading_agreement", "agree" },
                { "orders", orders }
            };
            parameters.AddOptional("deadline", deadline);

            if (validateOnly == true)
                parameters.AddOptionalParameter("validate", true);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/AddOrderBatch", KrakenExchange.RateLimiter.SpotRest, 0, true, requestBodyFormat: RequestBodyFormat.Json);
            return await _baseClient.SendAsync<KrakenBatchOrderResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenPlacedOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal quantity,
            decimal? price = null,
            decimal? secondaryPrice = null,
            decimal? leverage = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            bool? validateOnly = null,
            uint? userReference = null,
            string? clientOrderId = null,
            IEnumerable<OrderFlags>? flags = null,
            string? twoFactorPassword = null,
            TimeInForce? timeInForce = null,
            bool? reduceOnly = null,
            decimal? icebergQuanty = null,
            Trigger? trigger = null,
            SelfTradePreventionType? selfTradePreventionType = null,
            OrderType? closeOrderType = null,
            decimal? closePrice = null,
            decimal? secondaryClosePrice = null,
            string? pricePrefixOperator = null,
            string? priceSuffixOperator = null,
            string? secondaryPricePrefixOperator = null,
            string? secondaryPriceSuffixOperator = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "pair", symbol },
                { "volume", quantity.ToString(CultureInfo.InvariantCulture) },
                { "trading_agreement", "agree" }
            };
            parameters.AddEnum("type", side);
            parameters.AddEnum("ordertype", type);
            parameters.AddOptionalParameter("oflags", flags == null ? null: string.Join(",", flags.Select(EnumConverter.GetString)));
            if (price != null)
                parameters.AddOptionalParameter("price", $"{pricePrefixOperator}{price.Value.ToString(CultureInfo.InvariantCulture)}{priceSuffixOperator}");
            if (secondaryPrice != null)
                parameters.AddOptionalParameter("price2", $"{secondaryPricePrefixOperator}{secondaryPrice.Value.ToString(CultureInfo.InvariantCulture)}{secondaryPriceSuffixOperator}");
            parameters.AddOptionalParameter("userref", userReference);
            parameters.AddOptional("cl_ord_id", clientOrderId);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            parameters.AddOptionalParameter("leverage", leverage?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("starttm", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("expiretm", DateTimeConverter.ConvertToSeconds(expireTime));
            parameters.AddOptionalParameter("timeinforce", timeInForce?.ToString());
            parameters.AddOptionalParameter("reduce_only", reduceOnly);
            parameters.AddOptionalParameter("trigger", EnumConverter.GetString(trigger));
            parameters.AddOptionalParameter("displayvol", icebergQuanty?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("stptype", EnumConverter.GetString(selfTradePreventionType));
            parameters.AddOptionalEnum("close[ordertype]", closeOrderType);
            parameters.AddOptionalParameter("close[price]", closePrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("close[price2]", secondaryClosePrice?.ToString(CultureInfo.InvariantCulture));
            if (validateOnly == true)
                parameters.AddOptionalParameter("validate", true);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/AddOrder", KrakenExchange.RateLimiter.SpotRest, 0, true);
            var result = await _baseClient.SendAsync<KrakenPlacedOrder>(request, parameters, ct).ConfigureAwait(false);
            if (result)
                _baseClient.InvokeOrderPlaced(new OrderId { SourceObject = result.Data, Id = result.Data.OrderIds.FirstOrDefault() });
            return result;
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenEditOrder>> EditOrderAsync(
            string symbol,
            string orderId,
            decimal? quantity = null,
            decimal? icebergQuanty = null,
            decimal? price = null,
            decimal? secondaryPrice = null,
            IEnumerable<OrderFlags>? flags = null,
            DateTime? deadline = null,
            bool? cancelResponse = null,
            bool? validateOnly = null,
            uint? newClientOrderId = null,
            string? pricePrefixOperator = null,
            string? priceSuffixOperator = null,
            string? secondaryPricePrefixOperator = null,
            string? secondaryPriceSuffixOperator = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "pair", symbol },
                { "txid", orderId },
            };
            parameters.AddOptionalParameter("oflags", flags == null ? null : string.Join(",", flags.Select(EnumConverter.GetString)));
            parameters.AddOptionalParameter("volume", quantity?.ToString(CultureInfo.InvariantCulture));
            if (price != null)
                parameters.AddOptionalParameter("price", $"{pricePrefixOperator}{price.Value.ToString(CultureInfo.InvariantCulture)}{priceSuffixOperator}");
            if (secondaryPrice != null)
                parameters.AddOptionalParameter("price2", $"{secondaryPricePrefixOperator}{secondaryPrice.Value.ToString(CultureInfo.InvariantCulture)}{secondaryPriceSuffixOperator}");
            parameters.AddOptionalParameter("cancel_response", cancelResponse);
            parameters.AddOptionalParameter("deadline", deadline);
            parameters.AddOptionalParameter("userref", newClientOrderId);
            parameters.AddOptionalParameter("displayvol", icebergQuanty?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            if (validateOnly == true)
                parameters.AddOptionalParameter("validate", true);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/EditOrder", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenEditOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenCancelResult>> CancelOrderAsync(string orderId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            var parameters = new ParameterCollection()
            {
                {"txid", orderId}
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/CancelOrder", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenCancelResult>(request, parameters, ct).ConfigureAwait(false);
            if (result)
                _baseClient.InvokeOrderCanceled(new OrderId { SourceObject = result.Data, Id = orderId });
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenCancelResult>> CancelAllOrdersAsync(string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/CancelAll", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenCancelResult>(request, parameters, ct).ConfigureAwait(false);
            if (result)
                _baseClient.InvokeOrderCanceled(new OrderId { SourceObject = result.Data });
            return result;
        }

        #endregion

        #region Cancel All Orders After

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenCancelAfterResult>> CancelAllOrdersAfterAsync(TimeSpan cancelAfter, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("timeout", (int)cancelAfter.TotalSeconds);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/CancelAllOrdersAfter", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenCancelAfterResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
