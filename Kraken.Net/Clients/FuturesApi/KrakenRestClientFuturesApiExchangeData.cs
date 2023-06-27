using Kraken.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using System.Linq;
using CryptoExchange.Net;
using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Clients.FuturesApi
{
    public class KrakenRestClientFuturesApiExchangeData
    {
        private readonly KrakenRestClientFuturesApi _baseClient;

        internal KrakenRestClientFuturesApiExchangeData(KrakenRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
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

    }
}
