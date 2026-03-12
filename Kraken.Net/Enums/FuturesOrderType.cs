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
        /// ["<c>lmt</c>"] Limit order
        /// </summary>
        [Map("lmt", "limit")]
        Limit,
        /// <summary>
        /// ["<c>post</c>"] Post only limit order
        /// </summary>
        [Map("post")]
        PostOnlyLimit,
        /// <summary>
        /// ["<c>ioc</c>"] Immediate or cancel order
        /// </summary>
        [Map("ioc")]
        ImmediateOrCancel,
        /// <summary>
        /// ["<c>mkt</c>"] Market order with 1% price protection
        /// </summary>
        [Map("mkt")]
        Market,
        /// <summary>
        /// ["<c>stp</c>"] Stop order
        /// </summary>
        [Map("stp", "stop")]
        Stop,
        /// <summary>
        /// ["<c>take_profit</c>"] Take profit order
        /// </summary>
        [Map("take_profit")]
        TakeProfit,
        /// <summary>
        /// ["<c>trailing_stop</c>"] Trailing stop order
        /// </summary>
        [Map("trailing_stop")]
        TrailingStop
    }
}
