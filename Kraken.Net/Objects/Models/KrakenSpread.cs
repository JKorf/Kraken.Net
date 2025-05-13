using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;
using Kraken.Net.Converters;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Spread info
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<KrakenSpread>))]
    [SerializationModel]
    public record KrakenSpread
    {
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Best bid price
        /// </summary>
        [ArrayProperty(1)]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [ArrayProperty(2)]
        public decimal BestAskPrice { get; set; }
    }

    /// <summary>
    /// Stream spread data
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<KrakenStreamSpread>))]
    [SerializationModel]
    public record KrakenStreamSpread
    {
        /// <summary>
        /// Best bid price
        /// </summary>
        [ArrayProperty(0)]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [ArrayProperty(1)]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Best bid quantity
        /// </summary>
        [ArrayProperty(3)]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// Best ask quantity
        /// </summary>
        [ArrayProperty(4)]
        public decimal BestAskQuantity { get; set; }

    }
}
