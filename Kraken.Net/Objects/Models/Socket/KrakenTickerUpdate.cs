namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Ticker info
    /// </summary>
    [SerializationModel]
    public record KrakenTickerUpdate
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>bid</c>"] Price of best bid
        /// </summary>
        [JsonPropertyName("bid")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>bid_qty</c>"] Quantity of the best bid
        /// </summary>
        [JsonPropertyName("bid_qty")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>ask</c>"] Price of best ask
        /// </summary>
        [JsonPropertyName("ask")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>ask_qty</c>"] Quantity of the best ask
        /// </summary>
        [JsonPropertyName("ask_qty")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// ["<c>last</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>vwap</c>"] Volume weighted average price of last 24 hours
        /// </summary>
        [JsonPropertyName("vwap")]
        public decimal Vwap { get; set; }
        /// <summary>
        /// ["<c>low</c>"] Low price
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>high</c>"] High price
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>change</c>"] Price change in the last 24 hours
        /// </summary>
        [JsonPropertyName("change")]
        public decimal PriceChange { get; set; }
        /// <summary>
        /// ["<c>change_pct</c>"] Price change percentage in last 24 hours
        /// </summary>
        [JsonPropertyName("change_pct")]
        public decimal PriceChangePercentage { get; set; }
    }


}
