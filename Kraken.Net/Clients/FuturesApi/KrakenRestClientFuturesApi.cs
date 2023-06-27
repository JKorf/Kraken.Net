using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Futures;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.FuturesApi
{
    public class KrakenRestClientFuturesApi : RestApiClient
    {

        #region fields

        /// <inheritdoc />
        public new KrakenRestOptions ClientOptions => (KrakenRestOptions)base.ClientOptions;

        internal static TimeSyncState _timeSyncState = new TimeSyncState("Spot Api");
        #endregion

        #region Api clients
        public KrakenRestClientFuturesApiAccount Account { get; }
        public KrakenRestClientFuturesApiExchangeData ExchangeData { get; }
        public KrakenRestClientFuturesApiTrading Trading { get; }

        #endregion

        /// <inheritdoc />
        public string ExchangeName => "Kraken";

        #region ctor
        internal KrakenRestClientFuturesApi(ILogger logger, HttpClient? httpClient, KrakenRestOptions options)
            : base(logger, httpClient, options.Environment.FuturesRestBaseAddress, options, options.FuturesOptions)
        {
            //Account = new KrakenRestClientFuturesApiAccount(this);
            ExchangeData = new KrakenRestClientFuturesApiExchangeData(this);
            //Trading = new KrakenRestClientFuturesApiTrading(this);

            requestBodyFormat = RequestBodyFormat.FormData;
        }
        #endregion

        internal async Task<WebCallResult<U>> Execute<T, U>(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false)
            where T: KrakenFuturesResult<U>
        {
            var result = await SendRequestAsync<T>(url, method, ct, parameters, signed, requestWeight: weight, ignoreRatelimit: ignoreRatelimit).ConfigureAwait(false);
            if (!result)
                return result.AsError<U>(result.Error!);

            if (result.Data.Errors?.Any() == true)
            {
                if (result.Data.Errors.Count() > 1)
                    return result.AsError<U>(new ServerError(string.Join(", ", result.Data.Errors.Select(e => e.Code + ":" + e.Message))));
                else
                    return result.AsError<U>(new ServerError(result.Data.Errors.First().Code, result.Data.Errors.First().Message));
            }

            return result.As<U>(result.Data.Data);
        }

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => default; // ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ClientOptions.AutoTimestamp, ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;

    }
}
