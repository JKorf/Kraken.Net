using CryptoExchange.Net.Objects.Options;

namespace Kraken.Net.Objects.Options
{
    /// <summary>
    /// Options for the KrakenRestClient
    /// </summary>
    public class KrakenRestOptions : RestExchangeOptions<KrakenEnvironment>
    {
        /// <summary>
        /// Default options for new KrakenRestClients
        /// </summary>
        public static KrakenRestOptions Default { get; set; } = new KrakenRestOptions()
        {
            Environment = KrakenEnvironment.Live
        };

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
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();

        /// <summary>
        /// Options for the Futures API
        /// </summary>
        public RestApiOptions FuturesOptions { get; private set; } = new RestApiOptions();

        internal KrakenRestOptions Copy()
        {
            var options = Copy<KrakenRestOptions>();
            options.SpotOptions = SpotOptions.Copy<RestApiOptions>();
            options.FuturesOptions = FuturesOptions.Copy<RestApiOptions>();
            options.NonceProvider = NonceProvider;
            options.StaticTwoFactorAuthenticationPassword = StaticTwoFactorAuthenticationPassword;
            return options;
        }
    }
}
