using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Clients.MessageHandlers;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Models.Futures;
using Kraken.Net.Objects.Options;
using System.Net.Http.Headers;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal partial class KrakenRestClientFuturesApi : RestApiClient<KrakenEnvironment, KrakenFuturesAuthenticationProvider, KrakenCredentials>, IKrakenRestClientFuturesApi
    {
        #region fields

        /// <inheritdoc />
        public new KrakenRestOptions ClientOptions => (KrakenRestOptions)base.ClientOptions;
        protected override ErrorMapping ErrorMapping => KrakenErrors.FuturesMapping;
        protected override IRestMessageHandler MessageHandler { get; } = new KrakenRestFuturesMessageHandler(KrakenErrors.FuturesMapping);
        #endregion

        public override KrakenFuturesAuthenticationProvider? AuthenticationProvider 
        {
            get
            {
                if (!_authProviderInitialized)
                {
                    if (ApiCredentials?.Futures != null)
                        _authenticationProvider = CreateAuthenticationProvider(ApiCredentials);

                    _authProviderInitialized = true;
                }

                return _authenticationProvider;
            }
            protected set => base.AuthenticationProvider = value;
        }

        #region Api clients
        /// <inheritdoc />
        public IKrakenRestClientFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public IKrakenRestClientFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IKrakenRestClientFuturesApiTrading Trading { get; }

        #endregion

        #region ctor
        internal KrakenRestClientFuturesApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, KrakenRestOptions options)
            : base(loggerFactory, KrakenExchange.Metadata.Id, httpClient, options.Environment.FuturesRestBaseAddress, options, options.FuturesOptions)
        {
            Account = new KrakenRestClientFuturesApiAccount(this);
            ExchangeData = new KrakenRestClientFuturesApiExchangeData(this);
            Trading = new KrakenRestClientFuturesApiTrading(this);

            RequestBodyFormat = RequestBodyFormat.FormData;
            ParameterPositions[HttpMethod.Put] = HttpMethodParameterPosition.InUri;
            RequestBodyEmptyContent = "";
        }
        #endregion

        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(KrakenExchange._serializerContext));

        public IKrakenRestClientFuturesApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => KrakenExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        internal async Task<HttpResult<U>> SendAsync<T,U>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : KrakenFuturesResult<U>
        {
            var result = await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<U>(result);

            return HttpResult.Ok(result, result.Data.Data);
        }

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<KrakenFuturesResult>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail(result);

            return HttpResult.Ok(result);
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<KrakenFuturesResult<T>>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<T>(result);

            return HttpResult.Ok(result, result.Data.Data);
        }

        internal async Task<HttpResult<T>> SendRawAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T: class
            => await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

        /// <inheritdoc />
        protected override KrakenFuturesAuthenticationProvider CreateAuthenticationProvider(KrakenCredentials credentials)
            => new KrakenFuturesAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        /// <inheritdoc />
        protected override async Task<HttpResult<DateTime>> GetServerTimestampAsync()
        {
            var result = await ExchangeData.GetPlatformNotificationsAsync().ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            return HttpResult.Ok(result, result.Data.ServerTime);
        }
    }
}
