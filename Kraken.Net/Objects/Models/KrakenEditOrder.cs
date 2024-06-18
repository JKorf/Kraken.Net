using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Edited order info
    /// </summary>
    public record KrakenEditOrder
    {
        /// <summary>
        /// Order ids
        /// </summary>
        [JsonProperty("txid")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Descriptions
        /// </summary>
        [JsonProperty("descr")]
        public KrakenPlacedOrderDescription Descriptions { get; set; } = default!;
    }
}
