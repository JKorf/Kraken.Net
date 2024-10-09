using CryptoExchange.Net.Converters;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesOrderBookResult : KrakenFuturesResult<KrakenFuturesOrderBook>
    {
        [JsonPropertyName("orderBook")]
        public override KrakenFuturesOrderBook Data { get; set; } = null!;
    }

    /// <summary>
    /// Order book
    /// </summary>
    public record KrakenFuturesOrderBook
    {
        /// <summary>
        /// List of asks
        /// </summary>
        [JsonPropertyName("asks")]
        public IEnumerable<KrakenFuturesOrderBookEntry> Asks { get; set; } = Array.Empty<KrakenFuturesOrderBookEntry>();
        /// <summary>
        /// List of bids
        /// </summary>
        [JsonPropertyName("bids")]
        public IEnumerable<KrakenFuturesOrderBookEntry> Bids { get; set; } = Array.Empty<KrakenFuturesOrderBookEntry>();
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public record KrakenFuturesOrderBookEntry
    {
        /// <summary>
        /// Price
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantiy
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
    }
}
