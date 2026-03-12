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
        /// ["<c>online</c>"] Online
        /// </summary>
        [Map("online")]
        Online,
        /// <summary>
        /// ["<c>cancel_only</c>"] Only cancellation allowed
        /// </summary>
        [Map("cancel_only")]
        CancelOnly,
        /// <summary>
        /// ["<c>post_only</c>"] Only maker orders allowed
        /// </summary>
        [Map("post_only")]
        PostOnly,
        /// <summary>
        /// ["<c>limit_only</c>"] Only limit orders allowed
        /// </summary>
        [Map("limit_only")]
        LimitOnly,
        /// <summary>
        /// ["<c>reduce_only</c>"] Only allowed to reduce position
        /// </summary>
        [Map("reduce_only")]
        ReduceOnly,
        /// <summary>
        /// ["<c>delisted</c>"] Delisted
        /// </summary>
        [Map("delisted")]
        Delisted,
        /// <summary>
        /// ["<c>maintenance</c>"] Maintenance
        /// </summary>
        [Map("maintenance")]
        Maintenance,
        /// <summary>
        /// ["<c>work_in_progress</c>"] Work in process
        /// </summary>
        [Map("work_in_progress")]
        WorkInProcess
    }
}
