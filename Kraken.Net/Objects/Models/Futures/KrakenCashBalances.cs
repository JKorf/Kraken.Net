using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Cash balances
    /// </summary>
    public record KrakenCashBalances : KrakenBalances
    {
        /// <summary>
        /// Balances
        /// </summary>
        [JsonPropertyName("balances")]
        public Dictionary<string, decimal> Balances { get; set; } = new Dictionary<string, decimal>();
    }
}
