using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Ticker info
    /// </summary>
    public record KrakenTickerUpdate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Price of best bid
        /// </summary>
        [JsonPropertyName("bid")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Quantity of the best bid
        /// </summary>
        [JsonPropertyName("bid_qty")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// Price of best ask
        /// </summary>
        [JsonPropertyName("ask")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Quantity of the best ask
        /// </summary>
        [JsonPropertyName("ask_qty")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Volume weighted average price of last 24 hours
        /// </summary>
        [JsonPropertyName("vwap")]
        public decimal Vwap { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Price change in the last 24 hours
        /// </summary>
        [JsonPropertyName("change")]
        public decimal PriceChange { get; set; }
        /// <summary>
        /// Price change percentage in last 24 hours
        /// </summary>
        [JsonPropertyName("change_pct")]
        public decimal PriceChangePercentage { get; set; }
    }


}
