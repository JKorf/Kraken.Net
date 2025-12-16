using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Clients.MessageHandlers;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Options;
using System.Net.Http.Headers;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IKrakenRestClientSpotApi" />
    internal partial class KrakenRestClientSpotApi : RestApiClient, IKrakenRestClientSpotApi
    {
        #region fields

        /// <inheritdoc />
        public new KrakenRestOptions ClientOptions => (KrakenRestOptions)base.ClientOptions;

        internal static TimeSyncState _timeSyncState = new TimeSyncState("Spot Api");

        protected override ErrorMapping ErrorMapping => KrakenErrors.SpotMapping;
        protected override IRestMessageHandler MessageHandler { get; } = new KrakenRestSpotMessageHandler(KrakenErrors.SpotMapping);
        #endregion

        #region Api clients

        /// <inheritdoc />
        public IKrakenRestClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public IKrakenRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IKrakenRestClientSpotApiTrading Trading { get; }

        /// <inheritdoc />
        public IKrakenRestClientSpotApiEarn Earn { get; }

        /// <inheritdoc />
        public string ExchangeName => "Kraken";
        #endregion

        #region ctor
        internal KrakenRestClientSpotApi(ILogger logger, HttpClient? httpClient, KrakenRestOptions options)
            : base(logger, httpClient, options.Environment.SpotRestBaseAddress, options, options.SpotOptions)
        {
            Account = new KrakenRestClientSpotApiAccount(this);
            ExchangeData = new KrakenRestClientSpotApiExchangeData(this); 
            Trading = new KrakenRestClientSpotApiTrading(this);
            Earn = new KrakenRestClientSpotApiEarn(this);

            RequestBodyFormat = RequestBodyFormat.FormData;
        }
        #endregion

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => KrakenExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(SerializerOptions.WithConverters(KrakenExchange._serializerContext));

        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(KrakenExchange._serializerContext));

        /// <inheritdoc />
        protected override async ValueTask<bool> ShouldRetryRequestAsync<T>(IRateLimitGate? gate, WebCallResult<T> callResult, int tries)
        {
            if (await base.ShouldRetryRequestAsync(gate, callResult, tries).ConfigureAwait(false))
                return true;

            if (callResult.Error?.ErrorType != ErrorType.InvalidTimestamp)
                return false;

            if (tries <= 3)
            {
                _logger.Log(LogLevel.Warning, "Received nonce error; retrying request");
                await Task.Delay(25).ConfigureAwait(false);
                return true;
            }            

            return false;
        }

        internal async Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<KrakenResult>(BaseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result)
                return result.AsDatalessError(result.Error!);

            return result.AsDataless();
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) 
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) 
        {
            var result = await base.SendAsync<KrakenResult<T>>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result)
                return result.AsError<T>(result.Error!);

            return result.As(result.Data.Result);
        }

        /// <summary>
        /// Get the name of a symbol for Kraken based on the base and quote asset
        /// </summary>
        /// <param name="baseAsset"></param>
        /// <param name="quoteAsset"></param>
        /// <returns></returns>
        public string GetSymbolName(string baseAsset, string quoteAsset) => (baseAsset + quoteAsset).ToUpperInvariant();

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ClientOptions.AutoTimestamp, ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;

        public IKrakenRestClientSpotApiShared SharedClient => this;
    }
}
