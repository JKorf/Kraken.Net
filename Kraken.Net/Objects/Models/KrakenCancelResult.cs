using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models
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
        /// Pending cancelation orders
        /// </summary>
        public IEnumerable<long> Pending { get; set; } = Array.Empty<long>();
    }
}
