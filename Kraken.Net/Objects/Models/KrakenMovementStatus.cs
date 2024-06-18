using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Deposit status info
    /// </summary>
    public record KrakenMovementStatus
    {
        /// <summary>
        /// The name of the deposit method
        /// </summary>
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// The record of the asset
        /// </summary>
        [JsonProperty("aclass")]
        public string AssetClass { get; set; } = string.Empty;
        /// <summary>
        /// The asset name
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Reference id
        /// </summary>
        [JsonProperty("refid")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonProperty("txid")]
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// Info about the transaction
        /// </summary>
        [JsonProperty("info")]
        public string TransactionInfo { get; set; } = string.Empty;
        /// <summary>
        /// The quantity involved in the deposit
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The fee paid for the deposit
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// The timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Status of the transaction
        /// </summary>
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// Additional status info
        /// </summary>
        [JsonProperty("status-prop")]
        public string? AdditionalStatus { get; set; } = string.Empty;
        /// <summary>
        /// Withdrawal key name, as set up on your account
        /// </summary>
        [JsonProperty("key")]
        public string? Key { get; set; }
        /// <summary>
        /// Originators
        /// </summary>
        public IEnumerable<string>? Originators { get; set; } = Array.Empty<string>();
    }
}
