using Kraken.Net.Objects.Models.Futures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Account log snapshot update
    /// </summary>
    public class KrakenFuturesAccountLogsSnapshotUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// Account logs
        /// </summary>
        public IEnumerable<KrakenAccountLog> Logs { get; set; } = Array.Empty<KrakenAccountLog>();
    }

    /// <summary>
    /// New account log update
    /// </summary>
    public class KrakenFuturesAccountLogsUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// New entry
        /// </summary>
        [JsonProperty("new_entry")]
        public KrakenAccountLog NewEntry { get; set; } = null!;
    }
}
