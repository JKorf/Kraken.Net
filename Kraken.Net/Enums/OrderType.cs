using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// Limit order
        /// </summary>
        [Map("limit")]
        Limit,
        /// <summary>
        /// Symbol order
        /// </summary>
        [Map("market")]
        Market,
        /// <summary>
        /// Stop market order
        /// </summary>
        [Map("stop market")]
        StopMarket,
        /// <summary>
        /// Stop limit order
        /// </summary>
        [Map("stop limit")]
        StopLimit,
        /// <summary>
        /// Stop loss order
        /// </summary>
        [Map("stop-loss")]
        StopLoss,
        /// <summary>
        /// Take profit order
        /// </summary>
        [Map("take-profit")]
        TakeProfit,
        /// <summary>
        /// Stop loss profit order
        /// </summary>
        [Map("stop-loss-profit")]
        StopLossProfit,
        /// <summary>
        /// Stop loss profit limit order
        /// </summary>
        [Map("stop-loss-profit-limit")]
        StopLossProfitLimit,
        /// <summary>
        /// Stop loss limit order
        /// </summary>
        [Map("stop-loss-limit")]
        StopLossLimit,
        /// <summary>
        /// Take profit limit order
        /// </summary>
        [Map("take-profit-limit")]
        TakeProfitLimit,
        /// <summary>
        /// Trailing stop order
        /// </summary>
        [Map("trailing-stop")]
        TrailingStop,
        /// <summary>
        /// Trailing stop limit order
        /// </summary>
        [Map("trailing-stop-limit")]
        TrailingStopLimit,
        /// <summary>
        /// Stop loss and limit order
        /// </summary>
        [Map("stop-loss-and-limit")]
        StopLossAndLimit,
        /// <summary>
        /// Settle position
        /// </summary>
        [Map("settle-position")]
        SettlePosition
    }
}
