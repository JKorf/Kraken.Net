using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Net.Objects.Models;
using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.CommonObjects;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenRestClientSpotApiTrading : IKrakenClientSpotApiTrading
    {
        private readonly KrakenRestClientSpotApi _baseClient;

        internal KrakenRestClientSpotApiTrading(KrakenRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }


        /// <inheritdoc />
        public async Task<WebCallResult<OpenOrdersPage>> GetOpenOrdersAsync(uint? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var result = await _baseClient.Execute<OpenOrdersPage>(_baseClient.GetUri("0/private/OpenOrders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data.Open)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenClosedOrdersPage>> GetClosedOrdersAsync(uint? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.AddOptionalParameter("ofs", resultOffset);
            parameters.AddOptionalParameter("otp", twoFactorPassword);
            var result = await _baseClient.Execute<KrakenClosedOrdersPage>(_baseClient.GetUri("0/private/ClosedOrders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data.Closed)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        /// <inheritdoc />
        public Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrderAsync(string? orderId = null, uint? clientOrderId = null, bool? consolidateTaker = null, string? twoFactorPassword = null, CancellationToken ct = default)
            => GetOrdersAsync(orderId == null ? null : new[] { orderId }, clientOrderId, consolidateTaker, twoFactorPassword, ct);

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrdersAsync(IEnumerable<string>? orderIds = null, uint? clientOrderId = null, bool? consolidateTaker = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("txid", orderIds?.Any() == true ? string.Join(",", orderIds) : null);
            parameters.AddOptionalParameter("consolidate_taker", consolidateTaker);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var result = await _baseClient.Execute<Dictionary<string, KrakenOrder>>(_baseClient.GetUri("0/private/QueryOrders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenUserTradesPage>> GetUserTradesAsync(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, bool? consolidateTaker = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.AddOptionalParameter("ofs", resultOffset);
            parameters.AddOptionalParameter("consolidate_taker", consolidateTaker);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var result = await _baseClient.Execute<KrakenUserTradesPage>(_baseClient.GetUri("0/private/TradesHistory"), HttpMethod.Post, ct, parameters, true, weight: 2).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data.Trades)
                    item.Value.Id = item.Key;
            }

            return result;
        }

        /// <inheritdoc />
        public Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(string tradeId, string? twoFactorPassword = null, CancellationToken ct = default)
            => GetUserTradeDetailsAsync(new[] { tradeId }, twoFactorPassword, ct);


        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(IEnumerable<string> tradeIds, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("trades", true);
            parameters.AddOptionalParameter("txid", tradeIds?.Any() == true ? string.Join(",", tradeIds) : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var result = await _baseClient.Execute<Dictionary<string, KrakenUserTrade>>(_baseClient.GetUri("0/private/QueryTrades"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }


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
            uint? clientOrderId = null,
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
            CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                { "pair", symbol },
                { "type", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "ordertype", JsonConvert.SerializeObject(type, new OrderTypeConverter(false)) },
                { "volume", quantity.ToString(CultureInfo.InvariantCulture) },
                { "trading_agreement", "agree" }
            };
            parameters.AddOptionalParameter("oflags", flags == null ? null: string.Join(",", flags.Select(f => JsonConvert.SerializeObject(f, new OrderFlagsConverter(false)))));
            parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("userref", clientOrderId);
            parameters.AddOptionalParameter("price2", secondaryPrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            parameters.AddOptionalParameter("leverage", leverage?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("starttm", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("expiretm", DateTimeConverter.ConvertToSeconds(expireTime));
            parameters.AddOptionalParameter("timeinforce", timeInForce?.ToString());
            parameters.AddOptionalParameter("reduce_only", reduceOnly);
            parameters.AddOptionalParameter("trigger", EnumConverter.GetString(trigger));
            parameters.AddOptionalParameter("displayvol", icebergQuanty?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("stptype", EnumConverter.GetString(selfTradePreventionType));
            parameters.AddOptionalParameter("timeinforce", timeInForce?.ToString());
            parameters.AddOptionalParameter("close[ordertype]", closeOrderType == null? null: JsonConvert.SerializeObject(closeOrderType, new OrderTypeConverter(false)));
            parameters.AddOptionalParameter("close[price]", closePrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("close[price2]", secondaryClosePrice?.ToString(CultureInfo.InvariantCulture));
            if (validateOnly == true)
                parameters.AddOptionalParameter("validate", true);
            var result = await _baseClient.Execute<KrakenPlacedOrder>(_baseClient.GetUri("0/private/AddOrder"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
                _baseClient.InvokeOrderPlaced(new OrderId { SourceObject = result.Data, Id = result.Data.OrderIds.FirstOrDefault() });
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenCancelResult>> CancelOrderAsync(string orderId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            var parameters = new Dictionary<string, object>()
            {
                {"txid", orderId}
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var result = await _baseClient.Execute<KrakenCancelResult>(_baseClient.GetUri("0/private/CancelOrder"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
                _baseClient.InvokeOrderCanceled(new OrderId { SourceObject = result.Data, Id = orderId });
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenCancelResult>> CancelAllOrdersAsync(string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var result = await _baseClient.Execute<KrakenCancelResult>(_baseClient.GetUri("0/private/CancelAll"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
                _baseClient.InvokeOrderCanceled(new OrderId { SourceObject = result.Data });
            return result;
        }
    }
}
