//using System;
//using System.Collections.Generic;
//using CryptoExchange.Net.Interfaces;
//using CryptoExchange.Net.Objects;
//using Kraken.Net.Interfaces.Clients;

//namespace Kraken.Net.Objects
//{
//    /// <summary>
//    /// Options for the Kraken client
//    /// </summary>
//    public class KrakenClientOptions : ClientOptions
//    {
//        /// <summary>
//        /// Default options for the spot client
//        /// </summary>
//        public static KrakenClientOptions Default { get; set; } = new KrakenClientOptions();

//        /// <summary>
//        /// The static password configured as two-factor authentication for the API key. Will be send as otp parameter on private requests.
//        /// </summary>
//        public string? StaticTwoFactorAuthenticationPassword { get; set; }

//        /// <summary>
//        /// Optional nonce provider for signing requests. Careful providing a custom provider; once a nonce is sent to the server, every request after that needs a higher nonce than that
//        /// </summary>
//        public INonceProvider? NonceProvider { get; set; }

//        private RestApiClientOptions _spotApiOptions = new RestApiClientOptions(KrakenApiAddresses.Default.SpotRestClientAddress)
//        {
//            RateLimiters = new List<IRateLimiter>
//            {
//                    new RateLimiter()
//                        .AddApiKeyLimit(15, TimeSpan.FromSeconds(45), false, false)
//                        .AddEndpointLimit(new [] { "/private/AddOrder", "/private/CancelOrder", "/private/CancelAll", "/private/CancelAllOrdersAfter" }, 60, TimeSpan.FromSeconds(60), null, true),
//            }
//        };

//        /// <summary>
//        /// Spot API options
//        /// </summary>
//        public RestApiClientOptions SpotApiOptions
//        {
//            get => _spotApiOptions;
//            set => _spotApiOptions = new RestApiClientOptions(_spotApiOptions, value);
//        }

//        /// <summary>
//        /// ctor
//        /// </summary>
//        public KrakenClientOptions() : this(Default)
//        {
//        }

//        /// <summary>
//        /// ctor
//        /// </summary>
//        /// <param name="baseOn">Base the new options on other options</param>
//        internal KrakenClientOptions(KrakenClientOptions baseOn) : base(baseOn)
//        {
//            if (baseOn == null)
//                return;

//            NonceProvider = baseOn.NonceProvider;
//            StaticTwoFactorAuthenticationPassword = baseOn.StaticTwoFactorAuthenticationPassword;
//            _spotApiOptions = new RestApiClientOptions(baseOn.SpotApiOptions, null);
//        }
//    }

//    /// <summary>
//    /// Options for the Kraken socket client
//    /// </summary>
//    public class KrakenSocketClientOptions : ClientOptions
//    {
//        /// <summary>
//        /// Default options for the spot client
//        /// </summary>
//        public static KrakenSocketClientOptions Default { get; set; } = new KrakenSocketClientOptions();

//        /// <summary>
//        /// Optional nonce provider for signing requests. Careful providing a custom provider; once a nonce is sent to the server, every request after that needs a higher nonce than that
//        /// </summary>
//        public INonceProvider? NonceProvider { get; set; }

//        private KrakenSocketApiClientOptions _spotStreamsOptions = new KrakenSocketApiClientOptions(KrakenApiAddresses.Default.SpotSocketPublicAddress, KrakenApiAddresses.Default.SpotSocketPrivateAddress)
//        {
//            SocketSubscriptionsCombineTarget = 10
//        };

//        /// <summary>
//        /// Spot streams options
//        /// </summary>
//        public KrakenSocketApiClientOptions SpotStreamsOptions
//        {
//            get => _spotStreamsOptions;
//            set => _spotStreamsOptions = new KrakenSocketApiClientOptions(_spotStreamsOptions, value);
//        }

//        /// <summary>
//        /// ctor
//        /// </summary>
//        public KrakenSocketClientOptions() : this(Default)
//        {
//        }

//        /// <summary>
//        /// ctor
//        /// </summary>
//        /// <param name="baseOn">Base the new options on other options</param>
//        internal KrakenSocketClientOptions(KrakenSocketClientOptions baseOn) : base(baseOn)
//        {
//            if (baseOn == null)
//                return;

//            NonceProvider = baseOn.NonceProvider;
//            _spotStreamsOptions = new KrakenSocketApiClientOptions(baseOn.SpotStreamsOptions, null);
//        }
//    }

//    /// <summary>
//    /// Socket API options
//    /// </summary>
//    public class KrakenSocketApiClientOptions : SocketApiClientOptions
//    {
//        /// <summary>
//        /// The base address for the authenticated websocket
//        /// </summary>
//        public string BaseAddressAuthenticated { get; set; }

//        /// <summary>
//        /// ctor
//        /// </summary>
//#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
//        public KrakenSocketApiClientOptions()
//#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
//        {
//        }

//        /// <summary>
//        /// ctor
//        /// </summary>
//        /// <param name="baseAddress"></param>
//        /// <param name="baseAddressAuthenticated"></param>
//        internal KrakenSocketApiClientOptions(string baseAddress, string baseAddressAuthenticated) : base(baseAddress)
//        {
//            BaseAddressAuthenticated = baseAddressAuthenticated;
//        }

//        /// <summary>
//        /// ctor
//        /// </summary>
//        /// <param name="baseOn"></param>
//        /// <param name="newValues"></param>
//        internal KrakenSocketApiClientOptions(KrakenSocketApiClientOptions baseOn, KrakenSocketApiClientOptions? newValues) : base(baseOn, newValues)
//        {
//            BaseAddressAuthenticated = newValues?.BaseAddressAuthenticated ?? baseOn.BaseAddressAuthenticated;
//        }
//    }

//    /// <summary>
//    /// Options for the Kraken symbol order book
//    /// </summary>
//    public class KrakenOrderBookOptions : OrderBookOptions
//    {
//        /// <summary>
//        /// The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.
//        /// </summary>
//        public IKrakenSocketClient? SocketClient { get; set; }

//        /// <summary>
//        /// The limit of entries in the order book
//        /// </summary>
//        public int? Limit { get; set; }

//        /// <summary>
//        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
//        /// </summary>
//        public TimeSpan? InitialDataTimeout { get; set; }
//    }
//}

