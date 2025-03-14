using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
        /// <summary>
        /// Wallets
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
        /// Type
        /// </summary>
        [JsonPropertyName("type")]
        public WalletType WalletType { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
    }


}
