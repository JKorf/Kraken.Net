using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Futures position trade type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KrakenFuturesPositionTradeType>))]
    public enum KrakenFuturesPositionTradeType
    {
        /// <summary>
        /// User execution
        /// </summary>
        [Map("userExecution")]
        UserExecution,
        /// <summary>
        /// Liquidation
        /// </summary>
        [Map("liquidation")]
        Liquidation,
        /// <summary>
        /// Partial liquidation
        /// </summary>
        [Map("partialLiquidation")]
        PartialLiquidation,
        /// <summary>
        /// Assignment
        /// </summary>
        [Map("assignment")]
        Assignment,
        /// <summary>
        /// Unwind
        /// </summary>
        [Map("unwind")]
        Unwind,
        /// <summary>
        /// Unknown type
        /// </summary>
        [Map("unknown")]
        Unknown
    }
}
