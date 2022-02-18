using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Trade balance info
    /// </summary>
    public class KrakenTradeBalance
    {
        /// <summary>
        /// Combined balance
        /// </summary>
        [JsonProperty("eb")]
        public decimal CombinedBalance { get; set; }
        /// <summary>
        /// Trade balance
        /// </summary>
        [JsonProperty("tb")]
        public decimal TradeBalance { get; set; }
        /// <summary>
        /// Margin open positions
        /// </summary>
        [JsonProperty("m")]
        public decimal MarginOpenPositions { get; set; }
        /// <summary>
        /// Unrealized net profit in open positions
        /// </summary>
        [JsonProperty("n")]
        public decimal OpenPositionsUnrealizedNetProfit { get; set; }
        /// <summary>
        /// Cost basis for open positions
        /// </summary>
        [JsonProperty("c")]
        public decimal OpenPositionsCostBasis { get; set; }
        /// <summary>
        /// Open positions valuation
        /// </summary>
        [JsonProperty("v")]
        public decimal OpenPositionsValuation { get; set; }
        /// <summary>
        /// Equity
        /// </summary>
        [JsonProperty("e")]
        public decimal Equity { get; set; }
        /// <summary>
        /// Free margin
        /// </summary>
        [JsonProperty("mf")]
        public decimal FreeMargin { get; set; }
        /// <summary>
        /// Margin level
        /// </summary>
        [JsonProperty("ml")]
        public decimal MarginLevel { get; set; }
    }
}
