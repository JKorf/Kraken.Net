using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Symbol status
    /// </summary>
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
        ReduceOnly
    }
}
