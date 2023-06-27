using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using Kraken.Net.Objects.Models.Futures;
using CryptoExchange.Net;

namespace Kraken.Net.Clients.FuturesApi
{
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

    }
}
