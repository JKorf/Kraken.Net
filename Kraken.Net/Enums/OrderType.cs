using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderType>))]
    public enum OrderType
    {
        /// <summary>
        /// ["<c>limit</c>"] Limit order
        /// </summary>
        [Map("limit")]
        Limit,
        /// <summary>
        /// ["<c>market</c>"] Symbol order
        /// </summary>
        [Map("market")]
        Market,
        /// <summary>
        /// ["<c>stop market</c>"] Stop market order
        /// </summary>
        [Map("stop market")]
        StopMarket,
        /// <summary>
        /// ["<c>stop limit</c>"] Stop limit order
        /// </summary>
        [Map("stop limit")]
        StopLimit,
        /// <summary>
        /// ["<c>stop-loss</c>"] Stop loss order
        /// </summary>
        [Map("stop-loss")]
        StopLoss,
        /// <summary>
        /// ["<c>take-profit</c>"] Take profit order
        /// </summary>
        [Map("take-profit")]
        TakeProfit,
        /// <summary>
        /// ["<c>stop-loss-profit</c>"] Stop loss profit order
        /// </summary>
        [Map("stop-loss-profit")]
        StopLossProfit,
        /// <summary>
        /// ["<c>stop-loss-profit-limit</c>"] Stop loss profit limit order
        /// </summary>
        [Map("stop-loss-profit-limit")]
        StopLossProfitLimit,
        /// <summary>
        /// ["<c>stop-loss-limit</c>"] Stop loss limit order
        /// </summary>
        [Map("stop-loss-limit")]
        StopLossLimit,
        /// <summary>
        /// ["<c>take-profit-limit</c>"] Take profit limit order
        /// </summary>
        [Map("take-profit-limit")]
        TakeProfitLimit,
        /// <summary>
        /// ["<c>trailing-stop</c>"] Trailing stop order
        /// </summary>
        [Map("trailing-stop")]
        TrailingStop,
        /// <summary>
        /// ["<c>trailing-stop-limit</c>"] Trailing stop limit order
        /// </summary>
        [Map("trailing-stop-limit")]
        TrailingStopLimit,
        /// <summary>
        /// ["<c>stop-loss-and-limit</c>"] Stop loss and limit order
        /// </summary>
        [Map("stop-loss-and-limit")]
        StopLossAndLimit,
        /// <summary>
        /// ["<c>settle-position</c>"] Settle position
        /// </summary>
        [Map("settle-position")]
        SettlePosition
    }
}
