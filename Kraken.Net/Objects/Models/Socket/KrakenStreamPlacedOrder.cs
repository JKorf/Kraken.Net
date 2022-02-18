using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Placed order result
    /// </summary>
    public class KrakenStreamPlacedOrder: KrakenSocketResponseBase
    {
        /// <summary>
        /// Order description
        /// </summary>
        [JsonProperty("descr")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Placed order id
        /// </summary>
        [JsonProperty("txId")]
        public string OrderId { get; set; } = string.Empty;
    }
}
