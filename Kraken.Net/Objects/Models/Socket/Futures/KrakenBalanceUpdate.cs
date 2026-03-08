using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Balance update
    /// </summary>
    [SerializationModel]
    public record KrakenBalanceUpdate
    {
        /// <summary>
        /// ["<c>ledger_id</c>"] Ledger id
        /// </summary>
        [JsonPropertyName("ledger_id")]
        public string LedgerId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ref_id</c>"] Reference id
        /// </summary>
        [JsonPropertyName("ref_id")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Type
        /// </summary>
        [JsonPropertyName("type")]
        public BalanceUpdateType BalanceUpdateType { get; set; }
        /// <summary>
        /// ["<c>asset</c>"] Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>asset_class</c>"] Asset class
        /// </summary>
        [JsonPropertyName("asset_class")]
        public string AssetClass { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>category</c>"] Category
        /// </summary>
        [JsonPropertyName("category")]
        public BalanceUpdateCategory Category { get; set; }
        /// <summary>
        /// ["<c>wallet_type</c>"] Wallet type
        /// </summary>
        [JsonPropertyName("wallet_type")]
        public WalletType WalletType { get; set; }
        /// <summary>
        /// ["<c>wallet_id</c>"] Wallet id
        /// </summary>
        [JsonPropertyName("wallet_id")]
        public string WalletId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>balance</c>"] Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
    }


}
