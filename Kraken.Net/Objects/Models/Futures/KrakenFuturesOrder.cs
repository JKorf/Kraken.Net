using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Order info
    /// </summary>
    public class KrakenFuturesOrder : IKrakenFuturesOrder
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonProperty("cliOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonProperty("filled")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("lastUpdateTimestamp")]
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty("limitPrice")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonProperty("stopPrice")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Quantity remaining
        /// </summary>
        public decimal QuantityRemaining
        {
            get { return Quantity - QuantityFilled; }
            set { }
        }
        /// <summary>
        /// Reduce only
        /// </summary>
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonProperty("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public FuturesOrderType Type { get; set; }
    }
}
