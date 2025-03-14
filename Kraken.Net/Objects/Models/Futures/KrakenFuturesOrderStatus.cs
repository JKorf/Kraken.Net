using CryptoExchange.Net.Converters.SystemTextJson;
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

        [JsonPropertyName("status")]
        public KrakenFuturesOrderActiveStatus Status { get; set; }
        /// <summary>
        /// Update reason
        /// </summary>
        [JsonPropertyName("updateReason")]
        public string? UpdateReason { get; set; }
    }
}
