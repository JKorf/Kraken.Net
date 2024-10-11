﻿using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Order book update
    /// </summary>
    public record KrakenBookUpdate
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Asks in the book
        /// </summary>
        [JsonPropertyName("asks")]
        public IEnumerable<KrakenBookUpdateEntry> Asks { get; set; } = Array.Empty<KrakenBookUpdateEntry>();
        /// <summary>
        /// Bids in the book
        /// </summary>
        [JsonPropertyName("bids")]
        public IEnumerable<KrakenBookUpdateEntry> Bids { get; set; } = Array.Empty<KrakenBookUpdateEntry>();
        /// <summary>
        /// Checksum
        /// </summary>
        [JsonPropertyName("checksum")]
        public long Checksum { get; set; }
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    public record KrakenBookUpdateEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// The price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// The quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
    }
}
