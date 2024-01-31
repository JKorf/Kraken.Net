using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// User trades update
    /// </summary>
    public class KrakenFuturesUserTradesUpdate : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// Account
        /// </summary>
        public string Account { get; set; } = string.Empty;
        /// <summary>
        /// Trades
        /// </summary>
        [JsonProperty("fills")]
        public IEnumerable<KrakenFuturesUserTrade> Trades { get; set; } = Array.Empty<KrakenFuturesUserTrade>();
    }

    /// <summary>
    /// User trade update info
    /// </summary>
    public class KrakenFuturesUserTrade
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Sequence number
        /// </summary>
        [JsonProperty("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// Is buy
        /// </summary>
        public bool Buy { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonProperty("fill_id")]
        public string TradeId { get; set; } = string.Empty;
        /// <summary>
        /// Trade type
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonProperty("fill_type")]
        public TradeType TradeType { get; set; }
        /// <summary>
        /// Fee paid on trade
        /// </summary>
        [JsonProperty("fee_paid")]
        public decimal FeePaid { get; set; }
        /// <summary>
        /// Fee currency
        /// </summary>
        [JsonProperty("fee_currency")]
        public string FeeCurrency { get; set; } = string.Empty;
        /// <summary>
        /// Order type of the taker
        /// </summary>
        [JsonProperty("taker_order_type")]
        public string TakerOrderType { get; set; } = string.Empty;
        /// <summary>
        /// Order type
        /// </summary>
        [JsonProperty("order_type")]
        public string OrderType { get; set; } = string.Empty;
    }
}
