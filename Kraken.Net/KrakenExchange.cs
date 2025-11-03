using CryptoExchange.Net.Converters;
using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net
{
    /// <summary>
    /// Kraken exchange information and configuration
    /// </summary>
    public static class KrakenExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "Kraken";

        /// <summary>
        /// Exchange name
        /// </summary>
        public static string DisplayName => "Kraken";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        public static string ImageUrl { get; } = "https://raw.githubusercontent.com/JKorf/Kraken.Net/master/Kraken.Net/Icon/icon.png";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.kraken.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://docs.kraken.com/api/"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        /// <summary>
        /// Aliases for Kraken assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases =
            [
                new AssetAlias("XBT", "BTC"),
                new AssetAlias("XDG", "DOGE"),

                new AssetAlias("USD", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

        internal static JsonSerializerContext _serializerContext = JsonSerializerContextCache.GetOrCreate<KrakenSourceGenerationContext>();

        /// <summary>
        /// Format a base and quote asset to a Kraken recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

            if (tradingMode == TradingMode.Spot)
                return $"{baseAsset}{quoteAsset}";

            return $"{(tradingMode == TradingMode.PerpetualLinear ? "PF" : tradingMode == TradingMode.PerpetualInverse ? "PI" : tradingMode == TradingMode.DeliveryLinear ? "FF" : "FI")}_{baseAsset}{quoteAsset}" + (!deliverTime.HasValue ? string.Empty : ("_" + deliverTime.Value.ToString("yyMMdd")));
        }

        /// <summary>
        /// Rate limiter configuration for the Kraken API
        /// </summary>
        public static KrakenRateLimiters RateLimiter { get; } = new KrakenRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the Kraken API
    /// </summary>
    public class KrakenRateLimiters
    {
        internal IRateLimitGate SpotRest { get; private set; }
        internal IRateLimitGate SpotSocket { get; private set; }
        internal IRateLimitGate FuturesApi { get; private set; }
        internal IRateLimitGate FuturesSocket { get; private set; }

        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;

        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

        /// <summary>
        /// The Tier to use when calculating rate limits
        /// </summary>
        public RateLimitTier Tier { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal KrakenRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        /// <summary>
        /// Configure the rate limit with a different tier
        /// </summary>
        /// <param name="tier"></param>
        public void Configure(RateLimitTier tier)
        {
            Tier = tier;
            Initialize();
        }

        private void Initialize()
        {
            int limit = 15;
            double decay = 0.33;
            switch (Tier)
            {
                case RateLimitTier.Starter:
                    limit = 15;
                    decay = 0.33;
                    break;
                case RateLimitTier.Intermediate:
                    limit = 20;
                    decay = 0.5;
                    break;
                case RateLimitTier.Pro:
                    limit = 20;
                    decay = 1;
                    break;
            }

            SpotRest = new RateLimitGate("Spot Rest")
                                        .AddGuard(new RateLimitGuard(RateLimitGuard.PerEndpoint, new PathStartFilter("0/public"), 1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding))
                                        .AddGuard(new RateLimitGuard(RateLimitGuard.PerApiKey, new IGuardFilter[] { new AuthenticatedEndpointFilter(true) }, limit, TimeSpan.FromSeconds(1), RateLimitWindowType.Decay, decay));

            SpotSocket = new RateLimitGate("Spot Socket")
                                        .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Connection), 150, TimeSpan.FromMinutes(10), RateLimitWindowType.Sliding)); // 150 connections per sliding 10 minutes

            FuturesApi = new RateLimitGate("Futures Rest")
                                        .AddGuard(new RateLimitGuard(RateLimitGuard.PerApiKey, new PathStartFilter("derivatives/api"), 500, TimeSpan.FromSeconds(10), RateLimitWindowType.Fixed))
                                        .AddGuard(new RateLimitGuard(RateLimitGuard.PerApiKey, new PathStartFilter("api/history"), 100, TimeSpan.FromMinutes(10), RateLimitWindowType.Fixed));

            FuturesSocket = new RateLimitGate("Futures Socket")
                                        .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Connection), 100, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));

            SpotRest.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            SpotRest.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            SpotSocket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            SpotSocket.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            FuturesApi.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            FuturesApi.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            FuturesSocket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            FuturesSocket.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }
    }
}
