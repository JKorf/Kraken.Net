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
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "0/public/Time", KrakenExchange.RateLimiter.SpotRest, 1, false);
            var result = await _baseClient.SendAsync<KrakenServerTime>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);
            return HttpResult.Ok(result, result.Data.UnixTime);
        }

        #endregion

        #region Get System Status

        /// <inheritdoc />
        public async Task<HttpResult<KrakenSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "0/public/SystemStatus", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<KrakenSystemStatus>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, KrakenAssetInfo>>> GetAssetsAsync(IEnumerable<string>? assets = null, AClass? assetClass = null, bool? newAssetNameResponse = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("assetVersion", newAssetNameResponse);
            parameters.Add("aclass", assetClass);
            if (assets?.Any() == true)
                parameters.Add("asset", string.Join(",", assets));

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "0/public/Assets", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<Dictionary<string, KrakenAssetInfo>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, KrakenSymbol>>> GetSymbolsAsync(IEnumerable<string>? symbols = null, string? countryCode = null, AClass? assetClass = null, bool? newAssetNameResponse = null, string? executionVenue = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("country_code", countryCode);
            parameters.Add("aclass_base", assetClass);
            parameters.Add("assetVersion", newAssetNameResponse);
            parameters.Add("execution_venue", executionVenue);
            if (symbols?.Any() == true)
                parameters.Add("pair", string.Join(",", symbols));

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "0/public/AssetPairs", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<Dictionary<string, KrakenSymbol>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Ticker

        /// <inheritdoc />
        public Task<HttpResult<Dictionary<string, KrakenRestTick>>> GetTickerAsync(string symbol, CancellationToken ct = default)
            => GetTickersAsync(new[] { symbol }, null, ct);

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(IEnumerable<string>? symbols = null, AssetClass? assetClass = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            if (symbols?.Any() == true)
                parameters.AddParameter("pair", string.Join(",", symbols));

            parameters.Add("asset_class", assetClass);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "0/public/Ticker", KrakenExchange.RateLimiter.SpotRest, 1, false);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenRestTick>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            foreach (var item in result.Data)
                item.Value.Symbol = item.Key;

            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<KrakenKlinesResult>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? since = null, AClass? assetClass = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                {"pair", symbol},
            };
            parameters.Add("interval", interval);
            parameters.Add("asset_class", assetClass);
            parameters.Add("since", DateTimeConverter.ConvertToSeconds(since));

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "0/public/OHLC", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<KrakenKlinesResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<KrakenOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, AClass? assetClass = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                {"pair", symbol},
            };
            parameters.Add("asset_class", assetClass);
            parameters.Add("count", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "0/public/Depth", KrakenExchange.RateLimiter.SpotRest, 1, false);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenOrderBook>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<KrakenOrderBook>(result);

            return HttpResult.Ok(result, result.Data.First().Value);
        }

        #endregion

        #region Get Trade History

        /// <inheritdoc />
        public async Task<HttpResult<KrakenTradesResult>> GetTradeHistoryAsync(string symbol, DateTime? since = null, int? limit = null, AClass? assetClass = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                {"pair", symbol},
            };
            parameters.Add("asset_class", assetClass);
            parameters.Add("since", DateTimeConverter.ConvertToNanoseconds(since));
            parameters.Add("count", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "0/public/Trades", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<KrakenTradesResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Recent Spread

        /// <inheritdoc />
        public async Task<HttpResult<KrakenSpreadsResult>> GetRecentSpreadAsync(string symbol, DateTime? since = null, AClass? assetClass = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                {"pair", symbol},
            };
            parameters.Add("asset_class", assetClass);
            parameters.Add("since", DateTimeConverter.ConvertToSeconds(since));

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "0/public/Spread", KrakenExchange.RateLimiter.SpotRest, 1, false);
            return await _baseClient.SendAsync<KrakenSpreadsResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
