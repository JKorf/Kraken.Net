using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Objects.Errors;

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
        public async Task<HttpResult<OpenOrdersPage>> GetOpenOrdersAsync(uint? userReference = null, string? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("trades", true);
            parameters.Add("userref", userReference);
            parameters.Add("cl_ord_id", clientOrderId);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/OpenOrders", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<OpenOrdersPage>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                foreach (var item in result.Data.Open)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<HttpResult<KrakenClosedOrdersPage>> GetClosedOrdersAsync(
            uint? userRef = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? resultOffset = null, 
            string? clientOrderId = null,
            SearchTime? searchTime = null,
            bool? consolidateTaker = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("trades", true);
            parameters.Add("userref", userRef);
            parameters.Add("cl_ord_id", clientOrderId);
            parameters.Add("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.Add("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.Add("ofs", resultOffset);
            parameters.Add("closetime", searchTime);
            parameters.Add("consolidate_taker", consolidateTaker);
            parameters.Add("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/ClosedOrders", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenClosedOrdersPage>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                foreach (var item in result.Data.Closed)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public Task<HttpResult<Dictionary<string, KrakenOrder>>> GetOrderAsync(string? orderId = null, uint? clientOrderId = null, bool? consolidateTaker = null, bool? trades = null, string? twoFactorPassword = null, CancellationToken ct = default)
            => GetOrdersAsync(orderId == null ? null : new[] { orderId }, clientOrderId, consolidateTaker, trades, twoFactorPassword, ct);

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, KrakenOrder>>> GetOrdersAsync(IEnumerable<string>? orderIds = null, uint? clientOrderId = null, bool? consolidateTaker = null, bool? trades = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("userref", clientOrderId);
            parameters.Add("txid", orderIds?.Any() == true ? string.Join(",", orderIds) : null);
            parameters.Add("consolidate_taker", consolidateTaker);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            parameters.Add("trades", trades);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/QueryOrders", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenOrder>>(request, parameters, ct).ConfigureAwait(false);
            
            if (result.Success)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<KrakenUserTradesPage>> GetUserTradesAsync(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, bool? consolidateTaker = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("trades", true);
            parameters.Add("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.Add("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.Add("ofs", resultOffset);
            parameters.Add("consolidate_taker", consolidateTaker);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/TradesHistory", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenUserTradesPage>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                foreach (var item in result.Data.Trades)
                    item.Value.Id = item.Key;
            }

            return result;
        }

        #endregion

        #region Get User Trade Details

        /// <inheritdoc />
        public Task<HttpResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(string tradeId, string? twoFactorPassword = null, CancellationToken ct = default)
            => GetUserTradeDetailsAsync(new[] { tradeId }, twoFactorPassword, ct);


        #endregion

        #region Get User Trade Details

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(IEnumerable<string> tradeIds, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("trades", true);
            parameters.Add("txid", tradeIds?.Any() == true ? string.Join(",", tradeIds) : null);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
           
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/QueryTrades", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenUserTrade>>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<HttpResult<CallResult<KrakenPlacedBatchOrder>[]>> PlaceMultipleOrdersAsync(
            string symbol,
            IEnumerable<KrakenOrderRequest> orders,
            DateTime? deadline = null, 
            bool? validateOnly = null,
            AClass? assetClass = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "pair", symbol },
                { "trading_agreement", "agree" },
                { "orders", orders.ToArray() }
            };
            parameters.Add("deadline", deadline);
            parameters.Add("asset_class", assetClass);

            if (validateOnly == true)
                parameters.Add("validate", true);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/AddOrderBatch", KrakenExchange.RateLimiter.SpotRest, 0, true, requestBodyFormat: RequestBodyFormat.Json);
            var response = await _baseClient.SendAsync<KrakenBatchOrderResult>(request, parameters, ct).ConfigureAwait(false);
            if (!response.Success)
                return HttpResult.Fail<CallResult<KrakenPlacedBatchOrder>[]>(response);

            var result = new List<CallResult<KrakenPlacedBatchOrder>>();
            foreach (var item in response.Data.Orders)
            {
                if (string.IsNullOrEmpty(item.Error))
                {
                    result.Add(CallResult.Ok(item));
                }
                else
                {
                    var error = item.Error!.Split(':');
                    result.Add(CallResult.Fail<KrakenPlacedBatchOrder>(new ServerError(error[0], _baseClient.GetErrorInfo(error[0]!, string.Join(", ", error.Skip(1))))));
                }
            }

            if (result.All(x => !x.Success))
                return HttpResult.Fail<CallResult<KrakenPlacedBatchOrder>[]>(response, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, false, "All orders failed")), result.ToArray());

            return HttpResult.Ok(response, result.ToArray());
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<KrakenPlacedOrder>> PlaceOrderAsync(
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
            decimal? icebergQuantity = null,
            Trigger? trigger = null,
            SelfTradePreventionType? selfTradePreventionType = null,
            OrderType? closeOrderType = null,
            decimal? closePrice = null,
            decimal? secondaryClosePrice = null,
            string? pricePrefixOperator = null,
            string? priceSuffixOperator = null,
            string? secondaryPricePrefixOperator = null,
            string? secondaryPriceSuffixOperator = null,
            AClass? assetClass = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "pair", symbol },
                { "volume", quantity.ToString(CultureInfo.InvariantCulture) },
                { "trading_agreement", "agree" }
            };
            parameters.Add("type", side);
            parameters.Add("ordertype", type);
            parameters.Add("oflags", flags == null ? null: string.Join(",", flags.Select(EnumConverter.GetString)));
            if (price != null)
                parameters.Add("price", $"{pricePrefixOperator}{price.Value.ToString(CultureInfo.InvariantCulture)}{priceSuffixOperator}");
            if (secondaryPrice != null)
                parameters.Add("price2", $"{secondaryPricePrefixOperator}{secondaryPrice.Value.ToString(CultureInfo.InvariantCulture)}{secondaryPriceSuffixOperator}");
            parameters.Add("userref", userReference);
            parameters.Add("cl_ord_id", clientOrderId);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            parameters.Add("leverage", leverage?.ToString(CultureInfo.InvariantCulture));
            parameters.Add("starttm", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.Add("expiretm", DateTimeConverter.ConvertToSeconds(expireTime));
            parameters.Add("timeinforce", timeInForce?.ToString());
            parameters.Add("reduce_only", reduceOnly);
            parameters.Add("trigger", EnumConverter.GetString(trigger));
            parameters.Add("displayvol", icebergQuantity?.ToString(CultureInfo.InvariantCulture));
            parameters.Add("stptype", EnumConverter.GetString(selfTradePreventionType));
            parameters.Add("close[ordertype]", closeOrderType);
            parameters.Add("close[price]", closePrice?.ToString(CultureInfo.InvariantCulture));
            parameters.Add("close[price2]", secondaryClosePrice?.ToString(CultureInfo.InvariantCulture));
            if (validateOnly == true)
                parameters.Add("validate", true);
            parameters.Add("asset_class", assetClass);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/AddOrder", KrakenExchange.RateLimiter.SpotRest, 0, true);
            var result = await _baseClient.SendAsync<KrakenPlacedOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<HttpResult<KrakenEditOrder>> EditOrderAsync(
            string symbol,
            string orderId,
            decimal? quantity = null,
            decimal? icebergQuantity = null,
            decimal? price = null,
            decimal? secondaryPrice = null,
            IEnumerable<OrderFlags>? flags = null,
            DateTime? deadline = null,
            bool? cancelResponse = null,
            bool? validateOnly = null,
            uint? newClientOrderId = null,
            string? twoFactorPassword = null,
            string? pricePrefixOperator = null,
            string? priceSuffixOperator = null,
            string? secondaryPricePrefixOperator = null,
            string? secondaryPriceSuffixOperator = null,
            AClass? assetClass = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "pair", symbol },
                { "txid", orderId },
            };
            parameters.Add("oflags", flags == null ? null : string.Join(",", flags.Select(EnumConverter.GetString)));
            parameters.Add("volume", quantity?.ToString(CultureInfo.InvariantCulture));
            if (price != null)
                parameters.Add("price", $"{pricePrefixOperator}{price.Value.ToString(CultureInfo.InvariantCulture)}{priceSuffixOperator}");
            if (secondaryPrice != null)
                parameters.Add("price2", $"{secondaryPricePrefixOperator}{secondaryPrice.Value.ToString(CultureInfo.InvariantCulture)}{secondaryPriceSuffixOperator}");
            parameters.Add("cancel_response", cancelResponse);
            parameters.Add("deadline", deadline);
            parameters.Add("userref", newClientOrderId);
            parameters.Add("displayvol", icebergQuantity?.ToString(CultureInfo.InvariantCulture));
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            if (validateOnly == true)
                parameters.Add("validate", true);
            parameters.Add("asset_class", assetClass);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/EditOrder", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenEditOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<KrakenCancelResult>> CancelOrderAsync(string? orderId = null, string? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            if (orderId == null && clientOrderId == null)
                throw new ArgumentException("Either orderId or clientOrderId should be provided");

            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("txid", orderId);
            parameters.Add("cl_ord_id", clientOrderId);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/CancelOrder", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<HttpResult<KrakenCancelResult>> CancelAllOrdersAsync(string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/CancelAll", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenCancelResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders After

        /// <inheritdoc />
        public async Task<HttpResult<KrakenCancelAfterResult>> CancelAllOrdersAfterAsync(TimeSpan cancelAfter, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("timeout", (int)cancelAfter.TotalSeconds);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/CancelAllOrdersAfter", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenCancelAfterResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Multiple Orders

        /// <inheritdoc />
        public async Task<HttpResult<KrakenBatchCancelResult>> CancelMultipleOrdersAsync(IEnumerable<string>? orderIds = null, IEnumerable<string>? clientOrderIds = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.AddArray("orders", orderIds?.ToArray());
            parameters.AddArray("cl_ord_ids", clientOrderIds?.ToArray());
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/CancelOrderBatch", KrakenExchange.RateLimiter.SpotRest, 0, true, requestBodyFormat: RequestBodyFormat.Json);
            return await _baseClient.SendAsync<KrakenBatchCancelResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
