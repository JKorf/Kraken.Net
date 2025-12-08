using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Asset class
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AClass>))]
    public enum AClass
    {
        /// <summary>
        /// Tokenized asset (xstocks)
        /// </summary>
        [Map("tokenized_asset")]
        TokenizedAsset,
        /// <summary>
        /// Spot currency pairs
        /// </summary>
        [Map("currency")]
        Currency
    }
}
