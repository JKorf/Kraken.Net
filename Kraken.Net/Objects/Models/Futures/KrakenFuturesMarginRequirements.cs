using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesMarginRequirementsInternal : KrakenFuturesResult
    {
        [JsonProperty("initialMargin")]
        public decimal? InitialMargin { get; set; }
        [JsonProperty("price")]
        public decimal? Price { get; set; }
    }

    /// <summary>
    /// Minimal margin requirements
    /// </summary>
    public class KrakenFuturesMarginRequirements
    {
        /// <summary>
        /// Initial margin
        /// </summary>
        [JsonProperty("initialMargin")]
        public decimal? InitialMargin { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty("price")]
        public decimal? Price { get; set; }
    }
}
