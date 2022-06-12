namespace Kraken.Net.Objects.Models
{
    using System;

    using CryptoExchange.Net.Converters;

    using Kraken.Net.Converters;
    using Kraken.Net.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Staking Transaction Info
    /// </summary>
    public class KrakenStakingTransaction
    {
        /// <summary>
        /// Staking method as described by <see cref="KrakenStakingAsset.Method"/>.
        /// </summary>
        public string Method { get; set; } = null!;

        /// <summary>
        /// Asset code/name e.g. DOT
        /// </summary>
        [JsonProperty("asset")]
        public string Asset { get; set; } = null!;

        /// <summary>
        /// The reference ID of the transaction.
        /// </summary>
        [JsonProperty("refid")]
        public string ReferenceId { get; set; } = null!;

        /// <summary>
        /// The transaction amount.
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Transaction fee.
        /// </summary>
        [JsonProperty("fee")]
        public decimal? Fee { get; set; }

        /// <summary>
        /// Unix timestamp when the transaction was initiated.
        /// </summary>
        [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Transaction status.
        /// </summary>
        [JsonProperty("status"), JsonConverter(typeof(StakingTransactionStatusConverter))]
        public StakingTransactionStatus Status { get; set; }

        /// <summary>
        /// Transaction type.
        /// </summary>
        [JsonProperty("type"), JsonConverter(typeof(StakingTypeConverter))]
        public StakingType Type { get; set; }

        /// <summary>
        /// Unix timestamp from the start of bond period (applicable only to bonding transactions).
        /// </summary>
        [JsonProperty("bond_start"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? BondStart { get; set; }

        /// <summary>
        /// Unix timestamp from the start of bond period (applicable only to bonding transactions).
        /// </summary>
        [JsonProperty("bond_end"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? BondEnd { get; set; }
    }
}
