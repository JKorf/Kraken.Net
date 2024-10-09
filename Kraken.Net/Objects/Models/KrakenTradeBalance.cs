namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Trade balance info
    /// </summary>
    public record KrakenTradeBalance
    {
        /// <summary>
        /// Combined balance
        /// </summary>
        [JsonPropertyName("eb")]
        public decimal CombinedBalance { get; set; }
        /// <summary>
        /// Trade balance
        /// </summary>
        [JsonPropertyName("tb")]
        public decimal TradeBalance { get; set; }
        /// <summary>
        /// Margin open positions
        /// </summary>
        [JsonPropertyName("m")]
        public decimal MarginOpenPositions { get; set; }
        /// <summary>
        /// Unrealized net profit in open positions
        /// </summary>
        [JsonPropertyName("n")]
        public decimal OpenPositionsUnrealizedNetProfit { get; set; }
        /// <summary>
        /// Cost basis for open positions
        /// </summary>
        [JsonPropertyName("c")]
        public decimal OpenPositionsCostBasis { get; set; }
        /// <summary>
        /// Open positions valuation
        /// </summary>
        [JsonPropertyName("v")]
        public decimal OpenPositionsValuation { get; set; }
        /// <summary>
        /// Equity
        /// </summary>
        [JsonPropertyName("e")]
        public decimal Equity { get; set; }
        /// <summary>
        /// Free margin
        /// </summary>
        [JsonPropertyName("mf")]
        public decimal FreeMargin { get; set; }
        /// <summary>
        /// Margin level
        /// </summary>
        [JsonPropertyName("ml")]
        public decimal MarginLevel { get; set; }
    }
}
