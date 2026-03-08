using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesOrderStatusResult : KrakenFuturesResult<KrakenFuturesOrderStatus[]>
    {
        [JsonPropertyName("orders")]
        public override KrakenFuturesOrderStatus[] Data { get; set; } = Array.Empty<KrakenFuturesOrderStatus>();
    }

    /// <summary>
    /// Order status info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOrderStatus
    {
        /// <summary>
        /// ["<c>error</c>"] Order error
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        /// <summary>
        /// ["<c>order</c>"] Order details
        /// </summary>
        [JsonPropertyName("order")]
        public KrakenFuturesCachedOrder Order { get; set; } = null!;
        /// <summary>
        /// ["<c>status</c>"] Order status
        /// </summary>

        [JsonPropertyName("status")]
        public KrakenFuturesOrderActiveStatus Status { get; set; }
        /// <summary>
        /// ["<c>updateReason</c>"] Update reason
        /// </summary>
        [JsonPropertyName("updateReason")]
        public string? UpdateReason { get; set; }
    }
}
