using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Cash balances
    /// </summary>
    public class KrakenCashBalances : KrakenBalances
    {
        /// <summary>
        /// Balances
        /// </summary>
        public Dictionary<string, decimal> Balances { get; set; } = new Dictionary<string, decimal>();
    }
}
