using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Options;
using System;
using System.Collections.Generic;

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
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions()
        {
            RateLimiters = new List<IRateLimiter>
            {
                    new RateLimiter()
                        .AddApiKeyLimit(15, TimeSpan.FromSeconds(45), false, false)
                        .AddEndpointLimit(new [] { "/private/AddOrder", "/private/CancelOrder", "/private/CancelAll", "/private/CancelAllOrdersAfter" }, 60, TimeSpan.FromSeconds(60), null, true),
            }
        };

        internal KrakenRestOptions Copy()
        {
            var options = Copy<KrakenRestOptions>();
            options.SpotOptions = SpotOptions.Copy<RestApiOptions>();
            options.NonceProvider = NonceProvider;
            options.StaticTwoFactorAuthenticationPassword = StaticTwoFactorAuthenticationPassword;
            return options;
        }
    }
}
