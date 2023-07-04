using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Models.Futures;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    public class KrakenRestClientFuturesApi : RestApiClient
    {
        #region fields

        /// <inheritdoc />
        public new KrakenRestOptions ClientOptions => (KrakenRestOptions)base.ClientOptions;

        internal static TimeSyncState _timeSyncState = new TimeSyncState("Futures Api");
        #endregion

        #region Api clients
        /// <inheritdoc />
        public KrakenRestClientFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public KrakenRestClientFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public KrakenRestClientFuturesApiTrading Trading { get; }

        #endregion

        /// <inheritdoc />
        public string ExchangeName => "Kraken";

        #region ctor
        internal KrakenRestClientFuturesApi(ILogger logger, HttpClient? httpClient, KrakenRestOptions options)
            : base(logger, httpClient, options.Environment.FuturesRestBaseAddress, options, options.FuturesOptions)
        {
            Account = new KrakenRestClientFuturesApiAccount(this);
            ExchangeData = new KrakenRestClientFuturesApiExchangeData(this);
            Trading = new KrakenRestClientFuturesApiTrading(this);

            requestBodyFormat = RequestBodyFormat.FormData;
            ParameterPositions[HttpMethod.Put] = HttpMethodParameterPosition.InUri;
            arraySerialization = ArrayParametersSerialization.MultipleValues;
            requestBodyEmptyContent = "";
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

            if (!string.IsNullOrEmpty(result.Data.Error))
                return result.AsError<U>(new ServerError(result.Data.Error));

            return result.As(result.Data.Data);
        }

        internal async Task<WebCallResult<T>> Execute<T>(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false)
            where T : KrakenFuturesResult
        {
            var result = await SendRequestAsync<T>(url, method, ct, parameters, signed, requestWeight: weight, ignoreRatelimit: ignoreRatelimit).ConfigureAwait(false);
            if (!result)
                return result.AsError<T>(result.Error!);

            if (result.Data.Errors?.Any() == true)
            {
                if (result.Data.Errors.Count() > 1)
                    return result.AsError<T>(new ServerError(string.Join(", ", result.Data.Errors.Select(e => e.Code + ":" + e.Message))));
                else
                    return result.AsError<T>(new ServerError(result.Data.Errors.First().Code, result.Data.Errors.First().Message));
            }

            if (!string.IsNullOrEmpty(result.Data.Error))
                return result.AsError<T>(new ServerError(result.Data.Error));

            return result.As(result.Data);
        }

        internal async Task<WebCallResult> Execute(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false)
        {
            var result = await SendRequestAsync<KrakenFuturesResult>(url, method, ct, parameters, signed, requestWeight: weight, ignoreRatelimit: ignoreRatelimit).ConfigureAwait(false);
            if (!result)
                return result.AsDatalessError(result.Error!);

            if (result.Data.Errors?.Any() == true)
            {
                if (result.Data.Errors.Count() > 1)
                    return result.AsDatalessError(new ServerError(string.Join(", ", result.Data.Errors.Select(e => e.Code + ":" + e.Message))));
                else
                    return result.AsDatalessError(new ServerError(result.Data.Errors.First().Code, result.Data.Errors.First().Message));
            }

            if (!string.IsNullOrEmpty(result.Data.Error))
                return result.AsDatalessError(new ServerError(result.Data.Error));

            return result.AsDataless();
        }

        internal async Task<WebCallResult<T>> ExecuteBase<T>(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false)
            where T: class
        {
            return await SendRequestAsync<T>(url, method, ct, parameters, signed, requestWeight: weight, ignoreRatelimit: ignoreRatelimit).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Error ParseErrorResponse(JToken error)
        {
            var result = Deserialize<KrakenFuturesResult>(error);
            if (!result)
                return new ServerError(error.ToString());

            if (result.Data.Errors?.Any() == true)
            {
                var krakenError = result.Data.Errors.First();
                return new ServerError(krakenError.Code, krakenError.Message);
            }

            return new ServerError(result.Data!.Error);
        }

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenFuturesAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        /// <inheritdoc />
        protected override async Task<WebCallResult<DateTime>> GetServerTimestampAsync()
        {
            var result = await ExchangeData.GetPlatformNotificationsAsync().ConfigureAwait(false);
            return result.As(result.Data?.ServerTime ?? default);
        }

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ClientOptions.AutoTimestamp, ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;
    }
}
