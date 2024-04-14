using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces;

namespace Kraken.Net
{
    /// <summary>
    /// Kraken exchange information and helpers
    /// </summary>
    public static class KrakenExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "Kraken";

        /// <summary>
        /// Rate limiter for the Kraken API
        /// </summary>
        public static IKrakenAccessLimiter RateLimiter => RateLimiters;

        internal static KrakenRateLimiters RateLimiters { get; } = new KrakenRateLimiters();
    }

    /// <summary>
    /// Rate limiters configured for the Kraken API
    /// </summary>
    internal class KrakenRateLimiters : IKrakenAccessLimiter
    {
        public event Action<RateLimitEvent> RateLimitTriggered
        {
            add
            {
                SpotRestStarter.RateLimitTriggered += value;
                SpotRestIntermediate.RateLimitTriggered += value;
                SpotRestPro.RateLimitTriggered += value;
                FuturesApi.RateLimitTriggered += value;
                SpotSocket.RateLimitTriggered += value;
                FuturesSocket.RateLimitTriggered += value;
            }
            remove
            {
                SpotRestStarter.RateLimitTriggered -= value;
                SpotRestIntermediate.RateLimitTriggered -= value;
                SpotRestPro.RateLimitTriggered -= value;
                FuturesApi.RateLimitTriggered -= value;
                SpotSocket.RateLimitTriggered -= value;
                FuturesSocket.RateLimitTriggered -= value;
            }
        }

        public RateLimitTier Tier { get; set; } = RateLimitTier.Starter;


        /// <summary>
        /// Ratelimiter for Spot endpoints
        /// </summary>
        internal IRateLimitGate SpotRest => Tier switch
        {
            RateLimitTier.Starter => SpotRestStarter,
            RateLimitTier.Intermediate => SpotRestIntermediate,
            RateLimitTier.Pro => SpotRestPro,
            _ => SpotRestStarter
        };

        public IRateLimitGate SpotRestStarter { get; } = new RateLimitGate("Spot Rest")
                                                            .AddGuard(new RateLimitGuard((def, host, key) => host, new PathStartFilter("/0/public"), 1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding))
                                                            .AddGuard(new RateLimitGuard((def, host, key) => host, new ExactPathsFilter(new string[] { "0/private/AddOrderBatch", "0/private/AddOrder" }), 60, TimeSpan.FromSeconds(1), RateLimitWindowType.Decay, 1))
                                                            .AddGuard(new RateLimitGuard((def, host, key) => key!.ToString(), new IGuardFilter[] { new AuthenticatedEndpointFilter(true) }, 15, TimeSpan.FromSeconds(1), RateLimitWindowType.Decay, 0.33));

        public IRateLimitGate SpotRestIntermediate { get; } = new RateLimitGate("Spot Rest")
                                                            .AddGuard(new RateLimitGuard((def, host, key) => host, new PathStartFilter("/0/public"), 1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding))
                                                            .AddGuard(new RateLimitGuard((def, host, key) => host, new ExactPathsFilter(new string[] { "0/private/AddOrderBatch", "0/private/AddOrder" }), 80, TimeSpan.FromSeconds(1), RateLimitWindowType.Decay, 2.34))
                                                            .AddGuard(new RateLimitGuard((def, host, key) => key!.ToString(), new IGuardFilter[] { new AuthenticatedEndpointFilter(true) }, 20, TimeSpan.FromSeconds(1), RateLimitWindowType.Decay, 0.5));

        public IRateLimitGate SpotRestPro { get; } = new RateLimitGate("Spot Rest")
                                                            .AddGuard(new RateLimitGuard((def, host, key) => host, new PathStartFilter("/0/public"), 1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding))
                                                            .AddGuard(new RateLimitGuard((def, host, key) => host, new ExactPathsFilter(new string[] { "0/private/AddOrderBatch", "0/private/AddOrder" }), 225, TimeSpan.FromSeconds(1), RateLimitWindowType.Decay, 3.75))
                                                            .AddGuard(new RateLimitGuard((def, host, key) => key!.ToString(), new IGuardFilter[] { new AuthenticatedEndpointFilter(true) }, 20, TimeSpan.FromSeconds(1), RateLimitWindowType.Decay, 1));

        internal IRateLimitGate SpotSocket { get; } = new RateLimitGate("Spot Socket")
                                                                    .AddGuard(new RateLimitGuard((def, host, key) => host, new LimitItemTypeFilter(RateLimitItemType.Connection), 150, TimeSpan.FromMinutes(10), RateLimitWindowType.Sliding)); // 150 connections per sliding 10 minutes

        internal IRateLimitGate FuturesApi { get; } = new RateLimitGate("Futures Rest")
                                                            .AddGuard(new RateLimitGuard((def, host, key) => host, new PathStartFilter("/derivatives/api"), 500, TimeSpan.FromSeconds(10), RateLimitWindowType.Fixed))
                                                            .AddGuard(new RateLimitGuard((def, host, key) => host, new PathStartFilter("/api/history"), 100, TimeSpan.FromMinutes(10), RateLimitWindowType.Fixed));

        internal IRateLimitGate FuturesSocket { get; } = new RateLimitGate("Futures Socket")
                                                                    .AddGuard(new RateLimitGuard((def, host, key) => host, new LimitItemTypeFilter(RateLimitItemType.Connection), 100, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding)); 
    }
}
