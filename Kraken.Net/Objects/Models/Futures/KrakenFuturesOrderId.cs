using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Order id info
    /// </summary>
    public record KrakenFuturesOrderId
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonProperty("cliOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; } = string.Empty;
    }
}
