using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;
using Kraken.Net.Converters;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Fee level details
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<KrakenFeeEntry, KrakenSourceGenerationContext>))]
    [SerializationModel]
    public record KrakenFeeEntry
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
