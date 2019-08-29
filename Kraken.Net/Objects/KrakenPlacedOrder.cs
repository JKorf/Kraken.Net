using CryptoExchange.Net.Attributes;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Placed order info
    /// </summary>
    public class KrakenPlacedOrder
    {
        /// <summary>
        /// Order ids
        /// </summary>
        [JsonProperty("txid")]
        public string[] OrderIds { get; set; }
        /// <summary>
        /// Descriptions
        /// </summary>
        [JsonProperty("descr")]
        public KrakenPlacedOrderDescription Descriptions { get; set; }
    }

    /// <summary>
    /// Order descriptions
    /// </summary>
    public class KrakenPlacedOrderDescription
    {
        /// <summary>
        /// Order description
        /// </summary>
        [JsonProperty("order")]
        public string OrderDescription { get; set; }
        /// <summary>
        /// Close order description
        /// </summary>
        [JsonProperty("close")]
        [JsonOptionalProperty]
        public string CloseOrderDescription { get; set; }
    }
}
