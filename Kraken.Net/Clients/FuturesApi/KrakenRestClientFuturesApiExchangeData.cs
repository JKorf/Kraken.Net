using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net;
using Kraken.Net.Objects.Models.Futures;
using Kraken.Net.Enums;
using CryptoExchange.Net.Converters;
using Kraken.Net.Interfaces.Clients.FuturesApi;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class KrakenRestClientFuturesApiExchangeData : IKrakenRestClientFuturesApiExchangeData
    {
        private readonly KrakenRestClientFuturesApi _baseClient;

        internal KrakenRestClientFuturesApiExchangeData(KrakenRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesPlatfromNotificationResult>> GetPlatformNotificationsAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.Execute<KrakenFuturesPlatfromNotificationInternalResult>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/notifications")), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
            if (!result)
                return result.AsError<KrakenFuturesPlatfromNotificationResult>(result.Error!);

            return result.As(new KrakenFuturesPlatfromNotificationResult
            {
                Notifications = result.Data.Notifications,
                ServerTime = result.Data.ServerTime
            });
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFundingRate>>> GetHistoricalFundingRatesAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
            return await _baseClient.Execute<KrakenFundingRatesResult, IEnumerable<KrakenFundingRate>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v4/historicalfundingrates")), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFeeSchedule>>> GetFeeSchedulesAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenFeeSchedulesResult, IEnumerable<KrakenFeeSchedule>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/feeschedules")), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesSymbol>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenFuturesSymbolResult, IEnumerable<KrakenFuturesSymbol>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/instruments")), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesSymbolStatus>>> GetSymbolStatusAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenFuturesSymbolStatusResult, IEnumerable<KrakenFuturesSymbolStatus>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/instruments/status")), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesTrade>>> GetTradesAsync(string symbol, DateTime? startTime = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol }
            };
            parameters.AddOptionalParameter("lastTime", startTime?.ToString("u").Replace(" ", "T"));
            return await _baseClient.Execute<KrakenFuturesTradeResult, IEnumerable<KrakenFuturesTrade>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/history")), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol }
            };
            return await _baseClient.Execute<KrakenFuturesOrderBookResult, KrakenFuturesOrderBook>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/orderbook")), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesTicker>>> GetTickersAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenFuturesTickerResult, IEnumerable<KrakenFuturesTicker>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/tickers")), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesKlines>> GetKlinesAsync(TickType tickType, string symbol, FuturesKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("from", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("to", DateTimeConverter.ConvertToSeconds(endTime));
            return await _baseClient.ExecuteBase<KrakenFuturesKlines>(new Uri(_baseClient.BaseAddress.AppendPath($"api/charts/v1/{EnumConverter.GetString(tickType)}/{symbol}/{EnumConverter.GetString(interval)}")), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }
    }
}
