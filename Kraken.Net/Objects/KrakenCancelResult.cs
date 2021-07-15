using System;
using System.Collections.Generic;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Result of a cancel request
    /// </summary>
    public class KrakenCancelResult
    {
        /// <summary>
        /// Amount of canceled orders
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Pending cancellation orders
        /// </summary>
        [JsonOptionalProperty]
        public IEnumerable<long> Pending { get; set; } = Array.Empty<long>();
    }
}
