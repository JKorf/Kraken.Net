using CryptoExchange.Net.Objects.Options;

namespace Kraken.Net.Objects.Options
{
    /// <summary>
    /// Options for the KrakenRestClient
    /// </summary>
    public class KrakenRestOptions : RestExchangeOptions<KrakenEnvironment, KrakenCredentials>
    {
        /// <summary>
        /// Default options for new KrakenRestClients
        /// </summary>
        internal static KrakenRestOptions Default { get; set; } = new KrakenRestOptions()
        {
            Environment = KrakenEnvironment.Live
        };

        /// <summary>
        /// ctor
        /// </summary>
        public KrakenRestOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// The static password configured as two-factor authentication for the API key. Will be send as otp parameter on private requests.
        /// </summary>
        public string? StaticTwoFactorAuthenticationPassword { get; set; }

        /// <summary>
        /// Optional nonce provider for signing requests. Careful providing a custom provider; once a nonce is sent to the server, every request after that needs a higher nonce than that
        /// </summary>
        public INonceProvider? NonceProvider { get; set; }

        /// <summary>
        /// Options for the Spot API
        /// </summary>
        public RestApiOptions<KrakenCredentials> SpotOptions { get; private set; } = new RestApiOptions<KrakenCredentials>();

        /// <summary>
        /// Options for the Futures API
        /// </summary>
        public RestApiOptions<KrakenCredentials> FuturesOptions { get; private set; } = new RestApiOptions<KrakenCredentials>();

        internal KrakenRestOptions Set(KrakenRestOptions targetOptions)
        {
            targetOptions = base.Set<KrakenRestOptions>(targetOptions);
            targetOptions.StaticTwoFactorAuthenticationPassword = StaticTwoFactorAuthenticationPassword;
            targetOptions.NonceProvider = NonceProvider;
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            return targetOptions;
        }
    }
}
