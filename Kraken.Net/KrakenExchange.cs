using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
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
                                        .AddGuard(new RateLimitGuard((def, host, key) => def.Path + def.Method, new PathStartFilter("0/public"), 1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding))
                                        .AddGuard(new RateLimitGuard((def, host, key) => key!.ToString(), new IGuardFilter[] { new AuthenticatedEndpointFilter(true) }, limit, TimeSpan.FromSeconds(1), RateLimitWindowType.Decay, decay));

            SpotSocket = new RateLimitGate("Spot Socket")
                                        .AddGuard(new RateLimitGuard((def, host, key) => host, new LimitItemTypeFilter(RateLimitItemType.Connection), 150, TimeSpan.FromMinutes(10), RateLimitWindowType.Sliding)); // 150 connections per sliding 10 minutes

            FuturesApi = new RateLimitGate("Futures Rest")
                                        .AddGuard(new RateLimitGuard((def, host, key) => host, new PathStartFilter("derivatives/api"), 500, TimeSpan.FromSeconds(10), RateLimitWindowType.Fixed))
                                        .AddGuard(new RateLimitGuard((def, host, key) => host, new PathStartFilter("api/history"), 100, TimeSpan.FromMinutes(10), RateLimitWindowType.Fixed));

            FuturesSocket = new RateLimitGate("Futures Socket")
                                        .AddGuard(new RateLimitGuard((def, host, key) => host, new LimitItemTypeFilter(RateLimitItemType.Connection), 100, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));

            SpotRest.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            SpotSocket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            FuturesApi.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            FuturesSocket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
        }
    }
}
