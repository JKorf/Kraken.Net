using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;
using Kraken.Net.Converters;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesOrderBookResult : KrakenFuturesResult<KrakenFuturesOrderBook>
    {
        [JsonPropertyName("orderBook")]
        public override KrakenFuturesOrderBook Data { get; set; } = null!;
    }

    /// <summary>
    /// Order book
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOrderBook
    {
        /// <summary>
        /// List of asks
        /// </summary>
        [JsonPropertyName("asks")]
        public KrakenFuturesOrderBookEntry[] Asks { get; set; } = Array.Empty<KrakenFuturesOrderBookEntry>();
        /// <summary>
        /// List of bids
        /// </summary>
        [JsonPropertyName("bids")]
        public KrakenFuturesOrderBookEntry[] Bids { get; set; } = Array.Empty<KrakenFuturesOrderBookEntry>();
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<KrakenFuturesOrderBookEntry, KrakenSourceGenerationContext>))]
    [SerializationModel]
    public record KrakenFuturesOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Price
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
    }
}
