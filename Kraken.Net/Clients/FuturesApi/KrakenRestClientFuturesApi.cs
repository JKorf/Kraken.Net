using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Models.Futures;
using Kraken.Net.Objects.Options;
using System;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal partial class KrakenRestClientFuturesApi : RestApiClient, IKrakenRestClientFuturesApi
    {
        #region fields

        /// <inheritdoc />
        public new KrakenRestOptions ClientOptions => (KrakenRestOptions)base.ClientOptions;

        internal static TimeSyncState _timeSyncState = new TimeSyncState("Futures Api");

        protected override ErrorMapping ErrorMapping => KrakenErrors.FuturesMapping;
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

        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(SerializerOptions.WithConverters(KrakenExchange._serializerContext));

        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(KrakenExchange._serializerContext));

        public IKrakenRestClientFuturesApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => KrakenExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        internal async Task<WebCallResult<U>> SendToAddressAsync<T,U>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : KrakenFuturesResult<U>
        {
            var result = await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result)
                return result.AsError<U>(result.Error!);

            return result.As(result.Data.Data);
        }

        internal async Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<KrakenFuturesResult>(BaseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result)
                return result.AsDatalessError(result.Error!);

            return result.AsDataless();
        }

        internal async Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<KrakenFuturesResult<T>>(BaseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result)
                return result.AsError<T>(result.Error!);

            return result.As(result.Data.Data);
        }

        internal Task<WebCallResult<U>> SendAsync<T,U>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : KrakenFuturesResult<U>
            => SendToAddressAsync<T,U>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendRawAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T: class
            => await base.SendAsync<T>(BaseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

        protected override Error? TryParseError(RequestDefinition request, KeyValuePair<string, string[]>[] responseHeaders, IMessageAccessor accessor)
        {
            if (!accessor.IsValid)
                return new ServerError(ErrorInfo.Unknown);

            var result = accessor.Deserialize<KrakenFuturesResult>();
            if (!result)
                return new ServerError(ErrorInfo.Unknown);

            if (result.Data.Errors?.Any() == true)
            {
                var krakenError = result.Data.Errors.First();
                return new ServerError(krakenError.Code, GetErrorInfo(krakenError.Code, krakenError.Message));
            }

            if (result.Data.Error?.Length > 0)
                return new ServerError(result.Data.Error, GetErrorInfo(result.Data.Error));

            return null;
        }

        /// <inheritdoc />
        protected override Error ParseErrorResponse(int httpStatusCode, KeyValuePair<string, string[]>[] responseHeaders, IMessageAccessor accessor, Exception? exception)
        {
            if (!accessor.IsValid)
                return new ServerError(ErrorInfo.Unknown, exception: exception);

            var result = accessor.Deserialize<KrakenFuturesResult>();
            if (!result)
                return new ServerError(ErrorInfo.Unknown, exception: exception);

            if (result.Data.Errors?.Any() == true)
            {
                var krakenError = result.Data.Errors.First();
                return new ServerError(krakenError.Code, GetErrorInfo(krakenError.Code, krakenError.Message), exception);
            }

            if (result.Data.Error?.Length > 0)
                return new ServerError(result.Data.Error, GetErrorInfo(result.Data.Error), exception: exception);

            return new ServerError(ErrorInfo.Unknown, exception: exception);
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
