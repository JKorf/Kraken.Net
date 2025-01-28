using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Timestamp to use for searching
    /// </summary>
    public enum SearchTime
    {
        /// <summary>
        /// Open time
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// Close time
        /// </summary>
        [Map("close")]
        Close,
        /// <summary>
        /// Both open and close time
        /// </summary>
        [Map("both")]
        Both
    }
}
