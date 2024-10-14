using Kraken.Net.Objects.Models.Futures;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.FuturesApi;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class KrakenRestClientFuturesApiExchangeData : IKrakenRestClientFuturesApiExchangeData
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly KrakenRestClientFuturesApi _baseClient;

        internal KrakenRestClientFuturesApiExchangeData(KrakenRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Platform Notificiations

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesPlatfromNotificationResult>> GetPlatformNotificationsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/notifications", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            var result = await _baseClient.SendRawAsync<KrakenFuturesPlatfromNotificationInternalResult>(request, null, ct).ConfigureAwait(false);
            if (!result)
                return result.AsError<KrakenFuturesPlatfromNotificationResult>(result.Error!);

            return result.As(new KrakenFuturesPlatfromNotificationResult
            {
                Notifications = result.Data.Notifications,
                ServerTime = result.Data.ServerTime
            });
        }

        #endregion

        #region Get Historical Funding Rates

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFundingRate>>> GetHistoricalFundingRatesAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v4/historicalfundingrates", KrakenExchange.RateLimiter.FuturesApi, 1, false);
            return await _baseClient.SendAsync<KrakenFundingRatesResult, IEnumerable<KrakenFundingRate>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Fee Schedules

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFeeSchedule>>> GetFeeSchedulesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/feeschedules", KrakenExchange.RateLimiter.FuturesApi, 1, false);
            return await _baseClient.SendAsync<KrakenFeeSchedulesResult, IEnumerable<KrakenFeeSchedule>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesSymbol>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/instruments", KrakenExchange.RateLimiter.FuturesApi, 1, false);
            return await _baseClient.SendAsync<KrakenFuturesSymbolResult, IEnumerable<KrakenFuturesSymbol>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbol Status

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesSymbolStatus>>> GetSymbolStatusAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/instruments/status", KrakenExchange.RateLimiter.FuturesApi, 1, false);
            return await _baseClient.SendAsync<KrakenFuturesSymbolStatusResult, IEnumerable<KrakenFuturesSymbolStatus>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trades

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesTrade>>> GetTradesAsync(string symbol, DateTime? startTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            parameters.AddOptionalParameter("lastTime", startTime?.ToString("u").Replace(" ", "T"));
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/history", KrakenExchange.RateLimiter.FuturesApi, 1, false);
            return await _baseClient.SendAsync<KrakenFuturesTradeResult, IEnumerable<KrakenFuturesTrade>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/orderbook", KrakenExchange.RateLimiter.FuturesApi, 1, false);
            return await _baseClient.SendAsync<KrakenFuturesOrderBookResult, KrakenFuturesOrderBook>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/tickers/" + symbol, KrakenExchange.RateLimiter.FuturesApi, 1, false);
            return await _baseClient.SendAsync<KrakenFuturesTickerResult, KrakenFuturesTicker>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesTicker>>> GetTickersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/tickers", KrakenExchange.RateLimiter.FuturesApi, 1, false);
            return await _baseClient.SendAsync<KrakenFuturesTickersResult, IEnumerable<KrakenFuturesTicker>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesKlines>> GetKlinesAsync(TickType tickType, string symbol, FuturesKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("from", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("to", DateTimeConverter.ConvertToSeconds(endTime));

            var request = _definitions.GetOrCreate(HttpMethod.Get, $"api/charts/v1/{EnumConverter.GetString(tickType)}/{symbol}/{EnumConverter.GetString(interval)}", KrakenExchange.RateLimiter.FuturesApi, 1, false);
            return await _baseClient.SendRawAsync<KrakenFuturesKlines>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
