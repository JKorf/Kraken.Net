using System;
using System.Collections.Generic;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Kraken.Net.Interfaces.Clients;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Options for the Kraken client
    /// </summary>
    public class KrakenClientOptions : BaseRestClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static KrakenClientOptions Default { get; set; } = new KrakenClientOptions();

        /// <summary>
        /// The static password configured as two-factor authentication for the API key. Will be send as otp parameter on private requests.
        /// </summary>
        public string? StaticTwoFactorAuthenticationPassword { get; set; }

        /// <summary>
        /// Optional nonce provider for signing requests. Careful providing a custom provider; once a nonce is sent to the server, every request after that needs a higher nonce than that
        /// </summary>
        public INonceProvider? NonceProvider { get; set; }

        private readonly RestApiClientOptions _spotApiOptions = new RestApiClientOptions(KrakenApiAddresses.Default.RestClientAddress)
        {
            RateLimiters = new List<IRateLimiter>
            {
                    new RateLimiter()
                        .AddApiKeyLimit(15, TimeSpan.FromSeconds(45), false, false)
                        .AddEndpointLimit(new [] { "/private/AddOrder", "/private/CancelOrder", "/private/CancelAll", "/private/CancelAllOrdersAfter" }, 60, TimeSpan.FromSeconds(60), null, true),
            }
        };

        /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiClientOptions SpotApiOptions
        {
            get => _spotApiOptions;
            set => _spotApiOptions.Copy(_spotApiOptions, value);
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public KrakenClientOptions()
        {
            if (Default == null)
                return;

            Copy(this, Default);
        }

        /// <summary>
        /// Copy the values of the def to the input
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="def"></param>
        public new void Copy<T>(T input, T def) where T : KrakenClientOptions
        {
            base.Copy(input, def);

            input.NonceProvider = def.NonceProvider;
            input.StaticTwoFactorAuthenticationPassword = def.StaticTwoFactorAuthenticationPassword;

            input.SpotApiOptions = new RestApiClientOptions(def.SpotApiOptions);
        }
    }

    /// <summary>
    /// Options for the Kraken socket client
    /// </summary>
    public class KrakenSocketClientOptions : BaseSocketClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static KrakenSocketClientOptions Default { get; set; } = new KrakenSocketClientOptions()
        {
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// Optional nonce provider for signing requests. Careful providing a custom provider; once a nonce is sent to the server, every request after that needs a higher nonce than that
        /// </summary>
        public INonceProvider? NonceProvider { get; set; }

        private readonly KrakenSocketApiClientOptions _spotStreamsOptions = new KrakenSocketApiClientOptions(KrakenApiAddresses.Default.SocketClientPublicAddress, KrakenApiAddresses.Default.SocketClientPrivateAddress);
        /// <summary>
        /// Spot streams options
        /// </summary>
        public KrakenSocketApiClientOptions SpotStreamsOptions
        {
            get => _spotStreamsOptions;
            set => _spotStreamsOptions.Copy(_spotStreamsOptions, value);
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public KrakenSocketClientOptions()
        {
            if (Default == null)
                return;

            Copy(this, Default);
        }

        /// <summary>
        /// Copy the values of the def to the input
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="def"></param>
        public new void Copy<T>(T input, T def) where T : KrakenSocketClientOptions
        {
            base.Copy(input, def);

            input.SpotStreamsOptions = new KrakenSocketApiClientOptions();
            def.SpotStreamsOptions.Copy(input.SpotStreamsOptions, def.SpotStreamsOptions);
        }
    }

    /// <summary>
    /// Socket API options
    /// </summary>
    public class KrakenSocketApiClientOptions : ApiClientOptions
    {
        /// <summary>
        /// The base address for the authenticated websocket
        /// </summary>
        public string BaseAddressAuthenticated { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public KrakenSocketApiClientOptions()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="baseAddressAuthenticated"></param>
        public KrakenSocketApiClientOptions(string baseAddress, string baseAddressAuthenticated) : base(baseAddress)
        {
            BaseAddressAuthenticated = baseAddressAuthenticated;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseOn"></param>
        public KrakenSocketApiClientOptions(KrakenSocketApiClientOptions baseOn) : base(baseOn)
        {
            BaseAddressAuthenticated = baseOn.BaseAddressAuthenticated;
        }

        /// <inheritdoc />
        public new void Copy<T>(T input, T def) where T : KrakenSocketApiClientOptions
        {
            base.Copy(input, def);

            if (def.BaseAddressAuthenticated != null)
                input.BaseAddressAuthenticated = def.BaseAddressAuthenticated;
        }
    }

    /// <summary>
    /// Options for the Kraken symbol order book
    /// </summary>
    public class KrakenOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.
        /// </summary>
        public IKrakenSocketClient? SocketClient { get; set; }

        /// <summary>
        /// The limit of entries in the order book
        /// </summary>
        public int? Limit { get; set; }
    }
}

