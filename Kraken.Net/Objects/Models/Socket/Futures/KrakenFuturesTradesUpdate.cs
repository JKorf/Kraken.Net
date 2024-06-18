﻿using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Sockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Trades update
    /// </summary>
    public record KrakenFuturesTradesSnapshotUpdate : KrakenFuturesEvent
    {
        /// <summary>
        /// Trades
        /// </summary>
        public IEnumerable<KrakenFuturesTradeUpdate> Trades { get; set; } = Array.Empty<KrakenFuturesTradeUpdate>();
    }

    /// <summary>
    /// Trade info
    /// </summary>
    public record KrakenFuturesTradeUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Uid
        /// </summary>
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// Order side
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Trade type
        /// </summary>
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Sequence number
        /// </summary>
        [JsonProperty("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        public decimal Price { get; set; }
    }
}
