using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Snapshot data
    /// </summary>
    [SerializationModel]
    public record KrakenBalanceSnapshot
    {
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
        /// ["<c>balance</c>"] Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
        /// <summary>
        /// ["<c>wallets</c>"] Wallets
        /// </summary>
        [JsonPropertyName("wallets")]
        public KrakenBalanceSnapshotWallet[] Wallets { get; set; } = Array.Empty<KrakenBalanceSnapshotWallet>();
    }

    /// <summary>
    /// Wallet info
    /// </summary>
    [SerializationModel]
    public record KrakenBalanceSnapshotWallet
    {
        /// <summary>
        /// ["<c>type</c>"] Type
        /// </summary>
        [JsonPropertyName("type")]
        public WalletType WalletType { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>balance</c>"] Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
    }


}
