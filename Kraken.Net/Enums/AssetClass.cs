using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Asset class
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AssetClass>))]
    public enum AssetClass
    {
        /// <summary>
        /// Tokenized asset
        /// </summary>
        [Map("tokenized_asset")]
        TokenizedAsset,
        /// <summary>
        /// Forex
        /// </summary>
        [Map("forex")]
        Forex
    }
}
