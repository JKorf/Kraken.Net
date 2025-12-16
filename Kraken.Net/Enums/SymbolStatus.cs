using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Symbol status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolStatus>))]
    public enum SymbolStatus
    {
        /// <summary>
        /// Online
        /// </summary>
        [Map("online")]
        Online,
        /// <summary>
        /// Only cancellation allowed
        /// </summary>
        [Map("cancel_only")]
        CancelOnly,
        /// <summary>
        /// Only maker orders allowed
        /// </summary>
        [Map("post_only")]
        PostOnly,
        /// <summary>
        /// Only limit orders allowed
        /// </summary>
        [Map("limit_only")]
        LimitOnly,
        /// <summary>
        /// Only allowed to reduce position
        /// </summary>
        [Map("reduce_only")]
        ReduceOnly,
        /// <summary>
        /// Delisted
        /// </summary>
        [Map("delisted")]
        Delisted,
        /// <summary>
        /// Maintenance
        /// </summary>
        [Map("maintenance")]
        Maintenance,
        /// <summary>
        /// Work in process
        /// </summary>
        [Map("work_in_progress")]
        WorkInProcess
    }
}
