using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;


namespace Kraken.Net.Enums
{
    /// <summary>
    /// Futures order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesOrderType>))]
    public enum FuturesOrderType
    {
        /// <summary>
        /// Limit order
        /// </summary>
        [Map("lmt", "limit")]
        Limit,
        /// <summary>
        /// Post only limit order
        /// </summary>
        [Map("post")]
        PostOnlyLimit,
        /// <summary>
        /// Immediate or cancel order
        /// </summary>
        [Map("ioc")]
        ImmediateOrCancel,
        /// <summary>
        /// Market order with 1% price protection
        /// </summary>
        [Map("mkt")]
        Market,
        /// <summary>
        /// Stop order
        /// </summary>
        [Map("stp", "stop")]
        Stop,
        /// <summary>
        /// Take profit order
        /// </summary>
        [Map("take_profit")]
        TakeProfit,
        /// <summary>
        /// Trailing stop order
        /// </summary>
        [Map("trailing_stop")]
        TrailingStop
    }
}
