using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Futures position change
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KrakenFuturesPositionChange>))]
    public enum KrakenFuturesPositionChange
    {
        /// <summary>
        /// Position opened
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// Position closed
        /// </summary>
        [Map("close")]
        Close,
        /// <summary>
        /// Position increased
        /// </summary>
        [Map("increase")]
        Increase,
        /// <summary>
        /// Position decreased
        /// </summary>
        [Map("decrease")]
        Decrease,
        /// <summary>
        /// Position reversed
        /// </summary>
        [Map("reverse")]
        Reverse,
        /// <summary>
        /// Position did not change
        /// </summary>
        [Map("noChange")]
        NoChange,
        /// <summary>
        /// Unknown change
        /// </summary>
        [Map("unknown")]
        Unknown
    }
}
