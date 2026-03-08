namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Trade balance info
    /// </summary>
    [SerializationModel]
    public record KrakenTradeBalance
    {
        /// <summary>
        /// ["<c>eb</c>"] Combined balance
        /// </summary>
        [JsonPropertyName("eb")]
        public decimal CombinedBalance { get; set; }
        /// <summary>
        /// ["<c>tb</c>"] Trade balance
        /// </summary>
        [JsonPropertyName("tb")]
        public decimal TradeBalance { get; set; }
        /// <summary>
        /// ["<c>m</c>"] Margin open positions
        /// </summary>
        [JsonPropertyName("m")]
        public decimal MarginOpenPositions { get; set; }
        /// <summary>
        /// ["<c>n</c>"] Unrealized net profit in open positions
        /// </summary>
        [JsonPropertyName("n")]
        public decimal OpenPositionsUnrealizedNetProfit { get; set; }
        /// <summary>
        /// ["<c>c</c>"] Cost basis for open positions
        /// </summary>
        [JsonPropertyName("c")]
        public decimal OpenPositionsCostBasis { get; set; }
        /// <summary>
        /// ["<c>v</c>"] Open positions valuation
        /// </summary>
        [JsonPropertyName("v")]
        public decimal OpenPositionsValuation { get; set; }
        /// <summary>
        /// ["<c>e</c>"] Equity
        /// </summary>
        [JsonPropertyName("e")]
        public decimal Equity { get; set; }
        /// <summary>
        /// ["<c>mf</c>"] Free margin
        /// </summary>
        [JsonPropertyName("mf")]
        public decimal FreeMargin { get; set; }
        /// <summary>
        /// ["<c>ml</c>"] Margin level
        /// </summary>
        [JsonPropertyName("ml")]
        public decimal MarginLevel { get; set; }
    }
}
