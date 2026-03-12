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
        /// ["<c>tokenized_asset</c>"] Tokenized asset (xstocks)
        /// </summary>
        [Map("tokenized_asset")]
        TokenizedAsset,
        /// <summary>
        /// ["<c>currency</c>"] Spot currency pairs
        /// </summary>
        [Map("currency")]
        Currency
    }
}
