using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Net.Objects.Models;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenRestClientSpotApiExchangeData : IKrakenClientSpotApiExchangeData
    {
        private readonly KrakenRestClientSpotApi _baseClient;

        internal KrakenRestClientSpotApiExchangeData(KrakenRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.Execute<KrakenServerTime>(_baseClient.GetUri("0/public/Time"), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);
            if (!result)
                return result.AsError<DateTime>(result.Error!);
            return result.As<DateTime>(result.Data.UnixTime);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenSystemStatus>(_baseClient.GetUri("0/public/SystemStatus"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenAssetInfo>>> GetAssetsAsync(IEnumerable<string>? assets = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (assets?.Any() == true)
                parameters.AddOptionalParameter("asset", string.Join(",", assets));

            return await _baseClient.Execute<Dictionary<string, KrakenAssetInfo>>(_baseClient.GetUri("0/public/Assets"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenSymbol>>> GetSymbolsAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (symbols?.Any() == true)
                parameters.AddOptionalParameter("pair", string.Join(",", symbols));

            return await _baseClient.Execute<Dictionary<string, KrakenSymbol>>(_baseClient.GetUri("0/public/AssetPairs"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickerAsync(string symbol, CancellationToken ct = default)
            => GetTickersAsync(new[] { symbol }, ct);

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (symbols?.Any() == true)
                parameters.AddParameter("pair", string.Join(",", symbols));

            var result = await _baseClient.Execute<Dictionary<string, KrakenRestTick>>(_baseClient.GetUri("0/public/Ticker"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result)
                return result;

            foreach (var item in result.Data)
                item.Value.Symbol = item.Key;

            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenKlinesResult>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? since = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
                {"interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}
            };
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToSeconds(since));
            return await _baseClient.Execute<KrakenKlinesResult>(_baseClient.GetUri("0/public/OHLC"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("count", limit);
            var result = await _baseClient.Execute<Dictionary<string, KrakenOrderBook>>(_baseClient.GetUri("0/public/Depth"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result)
                return result.AsError<KrakenOrderBook>(result.Error!);
            return result.As(result.Data.First().Value);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenTradesResult>> GetTradeHistoryAsync(string symbol, DateTime? since = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToNanoseconds(since));
            return await _baseClient.Execute<KrakenTradesResult>(_baseClient.GetUri("0/public/Trades"), HttpMethod.Get, ct, parameters: parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenSpreadsResult>> GetRecentSpreadAsync(string symbol, DateTime? since = null, CancellationToken ct = default)
        {
            symbol.ValidateKrakenSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"pair", symbol},
            };
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToSeconds(since));
            return await _baseClient.Execute<KrakenSpreadsResult>(_baseClient.GetUri("0/public/Spread"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }
    }
}
