using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class KrakenRestClientSpotApiExchangeData : IKrakenRestClientSpotApiExchangeData
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly KrakenRestClientSpotApi _baseClient;

        internal KrakenRestClientSpotApiExchangeData(KrakenRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "0/public/Time", KrakenExchange.RateLimiter.SpotRest, 1, false);
            var result = await _baseClient.SendAsync<KrakenServerTime>(request, null, ct).ConfigureAwait(false);
            if (!result)
                return result.AsError<DateTime>(result.Error!);
            return result.As(result.Data.UnixTime);
        }

        #endregion

        #region Get System Status

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "0/public/SystemStatus", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<KrakenSystemStatus>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenAssetInfo>>> GetAssetsAsync(IEnumerable<string>? assets = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (assets?.Any() == true)
                parameters.AddOptionalParameter("asset", string.Join(",", assets));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "0/public/Assets", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<Dictionary<string, KrakenAssetInfo>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenSymbol>>> GetSymbolsAsync(IEnumerable<string>? symbols = null, string? countryCode = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("country_code", countryCode);
            if (symbols?.Any() == true)
                parameters.AddOptionalParameter("pair", string.Join(",", symbols));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "0/public/AssetPairs", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<Dictionary<string, KrakenSymbol>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Ticker

        /// <inheritdoc />
        public Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickerAsync(string symbol, CancellationToken ct = default)
            => GetTickersAsync(new[] { symbol }, ct);

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbols?.Any() == true)
                parameters.AddParameter("pair", string.Join(",", symbols));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "0/public/Ticker", KrakenExchange.RateLimiter.SpotRest, 1, false);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenRestTick>>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            foreach (var item in result.Data)
                item.Value.Symbol = item.Key;

            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenKlinesResult>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? since = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                {"pair", symbol},
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToSeconds(since));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "0/public/OHLC", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<KrakenKlinesResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("count", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "0/public/Depth", KrakenExchange.RateLimiter.SpotRest, 1, false);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenOrderBook>>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.AsError<KrakenOrderBook>(result.Error!);
            return result.As(result.Data.First().Value);
        }

        #endregion

        #region Get Trade History

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenTradesResult>> GetTradeHistoryAsync(string symbol, DateTime? since = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToNanoseconds(since));
            parameters.AddOptionalParameter("count", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "0/public/Trades", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<KrakenTradesResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Recent Spread

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenSpreadsResult>> GetRecentSpreadAsync(string symbol, DateTime? since = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToSeconds(since));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "0/public/Spread", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<KrakenSpreadsResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
