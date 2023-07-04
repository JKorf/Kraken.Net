using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using Kraken.Net.Objects.Models.Futures;
using CryptoExchange.Net;
using System.Globalization;
using CryptoExchange.Net.Converters;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    public class KrakenRestClientFuturesApiAccount
    {
        private readonly KrakenRestClientFuturesApi _baseClient;

        internal KrakenRestClientFuturesApiAccount(KrakenRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenBalances>>> GetBalancesAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenBalancesResult, Dictionary<string, KrakenBalances>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/accounts")), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesPnlCurrency>>> GetPnlCurrencyAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenFuturesPnlCurrencyResult, IEnumerable<KrakenFuturesPnlCurrency>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/pnlpreferences")), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult> SetPnlCurrencyAsync(string symbol, string pnlCurrency, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "pnlPreference", pnlCurrency },
                { "symbol", symbol }
            };
            return await _baseClient.Execute(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/pnlpreferences")), HttpMethod.Put, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult> TransferAsync(
            string currency, decimal quantity, string fromAccount, string toAccount, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "fromAccount", fromAccount },
                { "toAccount", toAccount },
                { "unit", currency }
            };
            return await _baseClient.Execute(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/transfer")), HttpMethod.Post, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenAccountLogResult>> GetAccountLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? fromId = null, int? toId = null, string? sort = null, string? type = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("before", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("count", limit);
            parameters.AddOptionalParameter("from", fromId);
            parameters.AddOptionalParameter("info", type);
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("sort", sort);
            parameters.AddOptionalParameter("to", toId);

            return await _baseClient.ExecuteBase<KrakenAccountLogResult>(new Uri(_baseClient.BaseAddress.AppendPath("api/history/v3/account-log")), HttpMethod.Get, ct, parameters, signed: true).ConfigureAwait(false);
        }
    }
}
