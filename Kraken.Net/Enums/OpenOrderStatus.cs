using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Status of an open order
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OpenOrderStatus>))]
    public enum OpenOrderStatus
    {
        /// <summary>
        /// ["<c>untouched</c>"] The entire size of the order is unfilled
        /// </summary>
        [Map("untouched")]
        Untouched,
        /// <summary>
        /// ["<c>partiallyFilled</c>"] The size of the order is partially but not entirely filled
        /// </summary>
        [Map("partiallyFilled")]
        PartiallyFilled
    }
}
