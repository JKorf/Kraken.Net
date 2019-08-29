using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Deposit status info
    /// </summary>
    public class KrakenDepositStatus
    {
        /// <summary>
        /// The name of the deposit method
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// The class of the asset
        /// </summary>
        [JsonProperty("aclass")]
        public string AssetClass { get; set; }
        /// <summary>
        /// The asset name
        /// </summary>
        public string Asset { get; set; }
        /// <summary>
        /// Reference id
        /// </summary>
        [JsonProperty("refid")]
        public string ReferenceId { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonProperty("txid")]
        public string TransactionId { get; set; }
        /// <summary>
        /// Info about the transaction
        /// </summary>
        [JsonProperty("info")]
        public string TransactionInfo { get; set; }
        /// <summary>
        /// The amount involved in the deposit
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// The fee paid for the deposit
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// The timestamp
        /// </summary>
        [JsonConverter(typeof(TimestampSecondsConverter))]
        [JsonProperty("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Status of the transaction
        /// </summary>
        public string Status { get; set; }

    }
}
