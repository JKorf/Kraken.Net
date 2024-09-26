using CryptoExchange.Net.Clients;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Models.Futures;
using Kraken.Net.Objects.Options;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal partial class KrakenRestClientFuturesApi : RestApiClient, IKrakenRestClientFuturesApi
    {
        #region fields

        /// <inheritdoc />
        public new KrakenRestOptions ClientOptions => (KrakenRestOptions)base.ClientOptions;

        internal static TimeSyncState _timeSyncState = new TimeSyncState("Futures Api");
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IKrakenRestClientFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public IKrakenRestClientFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IKrakenRestClientFuturesApiTrading Trading { get; }

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

            RequestBodyFormat = RequestBodyFormat.FormData;
            ParameterPositions[HttpMethod.Put] = HttpMethodParameterPosition.InUri;
            ArraySerialization = ArrayParametersSerialization.MultipleValues;
            RequestBodyEmptyContent = "";
        }
        #endregion

        public IKrakenRestClientFuturesApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            return $"{(tradingMode == TradingMode.PerpetualLinear ? "PF" : tradingMode == TradingMode.PerpetualInverse ? "PI" : tradingMode == TradingMode.DeliveryLinear ? "FF": "FI")}_{baseAsset.ToUpperInvariant()}{quoteAsset.ToUpperInvariant()}" + (!deliverTime.HasValue ? string.Empty : ("_" + deliverTime.Value.ToString("yyMMdd")));
        } 

        internal async Task<WebCallResult<U>> Execute<T, U>(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int weight = 0)
            where T : KrakenFuturesResult<U>
        {
            var result = await SendRequestAsync<T>(url, method, ct, parameters, signed, requestWeight: weight, gate: KrakenExchange.RateLimiter.FuturesApi).ConfigureAwait(false);
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
                return result.AsError<U>(new ServerError(result.Data.Error!));

            return result.As(result.Data.Data);
        }

        internal async Task<WebCallResult<T>> Execute<T>(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int weight = 0)
            where T : KrakenFuturesResult
        {
            var result = await SendRequestAsync<T>(url, method, ct, parameters, signed, requestWeight: weight, gate: KrakenExchange.RateLimiter.FuturesApi).ConfigureAwait(false);
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
                return result.AsError<T>(new ServerError(result.Data.Error!));

            return result.As(result.Data);
        }

        internal async Task<WebCallResult> Execute(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int requestWeight = 0)
        {
            var result = await SendRequestAsync<KrakenFuturesResult>(url, method, ct, parameters, signed, requestWeight: requestWeight, gate: KrakenExchange.RateLimiter.FuturesApi).ConfigureAwait(false);
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
                return result.AsDatalessError(new ServerError(result.Data.Error!));

            return result.AsDataless();
        }

        internal async Task<WebCallResult<T>> ExecuteBase<T>(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int requestWeight = 0)
            where T : class
        {
            return await SendRequestAsync<T>(url, method, ct, parameters, signed, requestWeight: requestWeight, gate: KrakenExchange.RateLimiter.FuturesApi).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Error ParseErrorResponse(int httpStatusCode, IEnumerable<KeyValuePair<string, IEnumerable<string>>> responseHeaders, IMessageAccessor accessor)
        {
            if (!accessor.IsJson)
                return new ServerError(accessor.GetOriginalString());

            var result = accessor.Deserialize<KrakenFuturesResult>();
            if (!result)
                return new ServerError(accessor.GetOriginalString());

            if (result.Data.Errors?.Any() == true)
            {
                var krakenError = result.Data.Errors.First();
                return new ServerError(krakenError.Code, krakenError.Message);
            }

            return new ServerError(result.Data!.Error ?? accessor.GetOriginalString());
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
