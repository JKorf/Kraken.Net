using Kraken.Net.Objects.Models.Futures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Account log snapshot update
    /// </summary>
    public record KrakenFuturesAccountLogsSnapshotUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// Account logs
        /// </summary>
        public IEnumerable<KrakenAccountLog> Logs { get; set; } = Array.Empty<KrakenAccountLog>();
    }

    /// <summary>
    /// New account log update
    /// </summary>
    public record KrakenFuturesAccountLogsUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// New entry
        /// </summary>
        [JsonProperty("new_entry")]
        public KrakenAccountLog NewEntry { get; set; } = null!;
    }
}
