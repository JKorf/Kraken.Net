using CryptoExchange.Net.RateLimiting;
using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Interfaces
{
    /// <inheritdoc />
    public interface IKrakenAccessLimiter : IAccessLimiter
    {
        /// <summary>
        /// Configure the rate limiting tier
        /// </summary>
        void Configure(RateLimitTier tier);
    }
}
