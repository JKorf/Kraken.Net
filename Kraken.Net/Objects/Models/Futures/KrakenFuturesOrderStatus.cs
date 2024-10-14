using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesOrderStatusResult : KrakenFuturesResult<IEnumerable<KrakenFuturesOrderStatus>>
    {
        [JsonPropertyName("orders")]
        public override IEnumerable<KrakenFuturesOrderStatus> Data { get; set; } = Array.Empty<KrakenFuturesOrderStatus>();
    }

    /// <summary>
    /// Order status info
    /// </summary>
    public record KrakenFuturesOrderStatus
    {
        /// <summary>
        /// Order error
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        /// <summary>
        /// Order details
        /// </summary>
        [JsonPropertyName("order")]
        public KrakenFuturesCachedOrder Order { get; set; } = null!;
        /// <summary>
        /// Order status
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("status")]
        public KrakenFuturesOrderActiveStatus Status { get; set; }
        /// <summary>
        /// Update reason
        /// </summary>
        [JsonPropertyName("updateReason")]
        public string? UpdateReason { get; set; }
    }
}
