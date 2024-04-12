using CryptoExchange.Net.RateLimiting;
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
                SpotApiStarter.RateLimitTriggered += value;
                SpotApiIntermediate.RateLimitTriggered += value;
                SpotApiPro.RateLimitTriggered += value;
            }
            remove
            {
                SpotApiStarter.RateLimitTriggered -= value;
                SpotApiIntermediate.RateLimitTriggered -= value;
                SpotApiPro.RateLimitTriggered -= value;
            }
        }

        public RateLimitTier Tier { get; set; } = RateLimitTier.Starter;


        /// <summary>
        /// Ratelimiter for Spot endpoints
        /// </summary>
        internal IRateLimitGate SpotApi => Tier switch
        {
            RateLimitTier.Starter => SpotApiStarter,
            RateLimitTier.Intermediate => SpotApiIntermediate,
            RateLimitTier.Pro => SpotApiPro,
            _ => SpotApiStarter
        };

        /// <summary>
        /// Ratelimiter for Spot order endpoints
        /// </summary>
        internal IRateLimitGate SpotOrderApi => Tier switch
        {
            RateLimitTier.Starter => SpotApiOrderStarter,
            RateLimitTier.Intermediate => SpotApiOrderIntermediate,
            RateLimitTier.Pro => SpotApiOrderPro,
            _ => SpotApiOrderStarter
        };

        private IRateLimitGate SpotApiStarter { get; } = new RateLimitGate("Rest Spot")
                                                                    .AddGuard(new PartialEndpointIndividualLimitGuard("/0/public/", 1, TimeSpan.FromSeconds(1)), RateLimitWindowType.Sliding)
                                                                    .AddGuard(new ApiKeyLimitGuard(15, TimeSpan.FromSeconds(1)), RateLimitWindowType.Decay, decayRate: 0.33);
        private IRateLimitGate SpotApiIntermediate { get; } = new RateLimitGate("Rest Spot")
                                                                    .AddGuard(new PartialEndpointIndividualLimitGuard("/0/public/", 1, TimeSpan.FromSeconds(1)), RateLimitWindowType.Sliding)
                                                                    .AddGuard(new ApiKeyLimitGuard(20, TimeSpan.FromSeconds(1)), RateLimitWindowType.Decay, decayRate: 0.5);
        private IRateLimitGate SpotApiPro { get; } = new RateLimitGate("Rest Spot")
                                                                    .AddGuard(new PartialEndpointIndividualLimitGuard("/0/public/", 1, TimeSpan.FromSeconds(1)), RateLimitWindowType.Sliding)
                                                                    .AddGuard(new ApiKeyLimitGuard(20, TimeSpan.FromSeconds(1)), RateLimitWindowType.Decay, decayRate: 1);

        private IRateLimitGate SpotApiOrderStarter { get; } = new RateLimitGate("Rest Spot Orders")
                                                                    .AddGuard(new TotalLimitGuard(60, TimeSpan.FromSeconds(1)), RateLimitWindowType.Decay, decayRate: 1);
        private IRateLimitGate SpotApiOrderIntermediate { get; } = new RateLimitGate("Rest Spot Orders")
                                                                    .AddGuard(new TotalLimitGuard(80, TimeSpan.FromSeconds(1)), RateLimitWindowType.Decay, decayRate: 2.34);
        private IRateLimitGate SpotApiOrderPro { get; } = new RateLimitGate("Rest Spot Orders")
                                                                    .AddGuard(new TotalLimitGuard(225, TimeSpan.FromSeconds(1)), RateLimitWindowType.Decay, decayRate: 3.75);

        internal IRateLimitGate FuturesApi { get; } = new RateLimitGate("Rest Futures")
                                                                    .AddGuard(new PartialEndpointTotalLimitGuard("/derivatives/api", 500, TimeSpan.FromSeconds(10)), RateLimitWindowType.Fixed)
                                                                    .AddGuard(new PartialEndpointTotalLimitGuard("/api/history", 100, TimeSpan.FromMinutes(10)), RateLimitWindowType.Fixed);

        internal IRateLimitGate SpotSocket { get; } = new RateLimitGate("Socket Spot")
                                                                    .AddGuard(new ConnectionLimitGuard(150, TimeSpan.FromMinutes(10)), RateLimitWindowType.Sliding) // 150 connections per sliding 10 minutes
                                                                    .WithSingleEndpointRateLimitType(RateLimitWindowType.Fixed);

        internal IRateLimitGate FuturesSocket { get; } = new RateLimitGate("Socket Futures")
                                                                    .AddGuard(new PartialEndpointIndividualLimitGuard("/", 100, TimeSpan.FromSeconds(1)), RateLimitWindowType.Fixed) // 10 requests per second
                                                                    .WithSingleEndpointRateLimitType(RateLimitWindowType.Fixed);
    }
}
