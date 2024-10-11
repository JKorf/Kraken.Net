using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Balance update
    /// </summary>
    public record KrakenBalanceUpdate
    {
        /// <summary>
        /// Ledger id
        /// </summary>
        [JsonPropertyName("ledger_id")]
        public string LedgerId { get; set; } = string.Empty;
        /// <summary>
        /// Reference id
        /// </summary>
        [JsonPropertyName("ref_id")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("type")]
        public BalanceUpdateType BalanceUpdateType { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Asset class
        /// </summary>
        [JsonPropertyName("asset_class")]
        public string AssetClass { get; set; } = string.Empty;
        /// <summary>
        /// Category
        /// </summary>
        [JsonPropertyName("category")]
        public BalanceUpdateCategory Category { get; set; }
        /// <summary>
        /// Wallet type
        /// </summary>
        [JsonPropertyName("wallet_type")]
        public WalletType WalletType { get; set; }
        /// <summary>
        /// Wallet id
        /// </summary>
        [JsonPropertyName("wallet_id")]
        public string WalletId { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
    }


}
