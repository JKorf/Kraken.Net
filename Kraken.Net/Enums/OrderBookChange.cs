using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order book event
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderBookChange>))]
    public enum OrderBookChange
    {
        /// <summary>
        /// ["<c>add</c>"] Add
        /// </summary>
        [Map("add")]
        Add,
        /// <summary>
        /// ["<c>modify</c>"] Modify
        /// </summary>
        [Map("modify")]
        Modify,
        /// <summary>
        /// ["<c>delete</c>"] Delete
        /// </summary>
        [Map("delete")]
        Delete
    }
}
