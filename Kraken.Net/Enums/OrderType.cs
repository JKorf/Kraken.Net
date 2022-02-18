namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Limit order
        /// </summary>
        Limit,
        /// <summary>
        /// Symbol order
        /// </summary>
        Market,
        /// <summary>
        /// Stop market order
        /// </summary>
        StopMarket,
        /// <summary>
        /// Stop limit order
        /// </summary>
        StopLimit,
        /// <summary>
        /// Stop loss order
        /// </summary>
        StopLoss,
        /// <summary>
        /// Take profit order
        /// </summary>
        TakeProfit,
        /// <summary>
        /// Stop loss profit order
        /// </summary>
        StopLossProfit,
        /// <summary>
        /// Stop loss profit limit order
        /// </summary>
        StopLossProfitLimit,
        /// <summary>
        /// Stop loss limit order
        /// </summary>
        StopLossLimit,
        /// <summary>
        /// Take profit limit order
        /// </summary>
        TakeProfitLimit,
        /// <summary>
        /// Trailing stop order
        /// </summary>
        TrailingStop,
        /// <summary>
        /// Trailing stop limit order
        /// </summary>
        TrailingStopLimit,
        /// <summary>
        /// Stop loss and limit order
        /// </summary>
        StopLossAndLimit,
        /// <summary>
        /// Settle position
        /// </summary>
        SettlePosition
    }
}
