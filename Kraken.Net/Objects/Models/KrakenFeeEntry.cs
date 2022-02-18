using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Fee level details
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenFeeEntry
    {
        /// <summary>
        /// The minimal volume for this level
        /// </summary>
        [ArrayProperty(0)]
        public int Volume { get; set; }
        /// <summary>
        /// The fee percentage for this level
        /// </summary>
        [ArrayProperty(1)]
        public decimal FeePercentage { get; set; }
    }
}
