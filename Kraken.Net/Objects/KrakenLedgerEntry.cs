using System;
using CryptoExchange.Net.Converters;
using Kraken.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Ledger entry info
    /// </summary>
    public class KrakenLedgerEntry
    {
        /// <summary>
        /// Reference id
        /// </summary>
        [JsonProperty("refid")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The type of entry
        /// </summary>
        [JsonConverter(typeof(LedgerEntryTypeConverter))]
        public LedgerEntryType Type { get; set; }

        /// <summary>
        /// Class of the asset
        /// </summary>
        [JsonProperty("aclass")]
        public string AssetClass { get; set; } = string.Empty;

        /// <summary>
        /// Name of the asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// The quantity of the entry
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Fee paid
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// Resulting balance
        /// </summary>
        [JsonProperty("balance")]
        public decimal BalanceAfter { get; set; }
    }
}
