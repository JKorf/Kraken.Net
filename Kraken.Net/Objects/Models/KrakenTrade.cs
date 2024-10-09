﻿using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public record KrakenTrade
    {
        /// <summary>
        /// Price of the trade
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity of the trade
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp of trade
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [ArrayProperty(3), JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [ArrayProperty(4), JsonConverter(typeof(EnumConverter))]
        public OrderTypeMinimal Type { get; set; }

        /// <summary>
        /// Misc info
        /// </summary>
        [ArrayProperty(5)]
        public string Misc { get; set; } = string.Empty;
    }
}
