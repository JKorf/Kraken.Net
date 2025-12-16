using CryptoExchange.Net.Converters;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Order book
    /// </summary>
    [SerializationModel]
    public record KrakenOrderBook
    {
        /// <summary>
        /// Asks in the book
        /// </summary>
        [JsonPropertyName("asks")]
        public KrakenOrderBookEntry[] Asks { get; set; } = Array.Empty<KrakenOrderBookEntry>();
        /// <summary>
        /// Bids in the book
        /// </summary>
        [JsonPropertyName("bids")]
        public KrakenOrderBookEntry[] Bids { get; set; } = Array.Empty<KrakenOrderBookEntry>();
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<KrakenOrderBookEntry>))]
    [SerializationModel]
    public record KrakenOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Price of the entry
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }

        /// <summary>
        /// Price of the entry as a string literal
        /// </summary>
        [ArrayProperty(0)]
        public string RawPrice { get; set; } = string.Empty;
        /// <summary>
        /// Quantity of the entry
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Quantity of the entry as a string literal
        /// </summary>
        [ArrayProperty(1)]
        public string RawQuantity { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp of change
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Stream order book
    /// </summary>
    [SerializationModel]
    public record KrakenStreamOrderBook
    {
        /// <summary>
        /// Asks
        /// </summary>
        [JsonPropertyName("as")]
        public KrakenStreamOrderBookEntry[] Asks { get; set; } = Array.Empty<KrakenStreamOrderBookEntry>();
        /// <summary>
        /// Bids
        /// </summary>
        [JsonPropertyName("bs")]
        public KrakenStreamOrderBookEntry[] Bids { get; set; } = Array.Empty<KrakenStreamOrderBookEntry>();

        /// <summary>
        /// Checksum
        /// </summary>
        [JsonPropertyName("checksum")]
        public uint? Checksum { get; set; }

        /// <summary>
        /// Is this a snapshot?
        /// </summary>
        [JsonPropertyName("snapshot")]
        internal bool Snapshot { get; set; }
    }

    /// <summary>
    /// Stream order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<KrakenStreamOrderBookEntry>))]
    [SerializationModel]
    public record KrakenStreamOrderBookEntry : ISymbolOrderSequencedBookEntry
    {
        /// <summary>
        /// Price of the entry
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }

        /// <summary>
        /// Price of the entry as a string literal
        /// </summary>
        [ArrayProperty(0)]
        public string RawPrice { get; set; } = string.Empty;

        /// <summary>
        /// Quantity of the entry
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Quantity of the entry as a string literal
        /// </summary>
        [ArrayProperty(1)]
        public string RawQuantity { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of the entry
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Type of update
        /// </summary>
        [ArrayProperty(3)]
        public string UpdateType { get; set; } = string.Empty;

        /// <summary>
        /// Sequence
        /// </summary>
        [JsonIgnore]
        public long Sequence
        {
            get => Timestamp.Ticks;
            set => Timestamp = new DateTime(value);
        }
    }
}
