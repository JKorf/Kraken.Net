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
        /// The rate limiting tier
        /// </summary>
        RateLimitTier Tier { get; set; }
    }
}
