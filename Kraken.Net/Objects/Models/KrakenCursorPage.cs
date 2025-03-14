using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Data page with a cusor for pagination
    /// </summary>
    [SerializationModel]
    public record KrakenCursorPage<T>
    {
        /// <summary>
        /// Cursor for the next page
        /// </summary>
        [JsonPropertyName("next_cursor")]
        public string? NextCursor { get; set; }

        /// <summary>
        /// Page data
        /// </summary>
        [JsonPropertyName("items")]
        public T[] Items { get; set; } = Array.Empty<T>();
        [JsonPropertyName("withdrawals")]
        internal T[] Withdrawals { set => Items = value; }
        [JsonPropertyName("deposits")]
        internal T[] Deposits { set => Items = value; }
    }
}
