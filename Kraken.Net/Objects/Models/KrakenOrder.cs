using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Order info
    /// </summary>
    public class KrakenOrder
    {
        /// <summary>
        /// The id of the order
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Reference id
        /// </summary>
        [JsonProperty("refid")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// Client reference id
        /// </summary>
        [JsonProperty("userref")]
        public string ClientOrderId { get; set; } = string.Empty;
        /// <summary>
        /// Status of the order
        /// </summary>
        [JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Open timestamp
        /// </summary>
        [JsonProperty("opentm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Start timestamp
        /// </summary>
        [JsonProperty("starttm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Expire timestamp
        /// </summary>
        [JsonProperty("expiretm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// Close timestamp
        /// </summary>
        [JsonProperty("closetm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? CloseTime { get; set; }

        /// <summary>
        /// Order details
        /// </summary>
        [JsonProperty("descr")]
        public KrakenOrderInfo OrderDetails { get; set; } = default!;
        /// <summary>
        /// Quantity of the order
        /// </summary>
        [JsonProperty("vol")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Filled quantity
        /// </summary>
        [JsonProperty("vol_exec")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Cost of the order
        /// </summary>
        [JsonProperty("cost")]
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// Average price of the order
        /// </summary>
        [JsonProperty("price")]
        public decimal AveragePrice { get; set; }
        [JsonProperty("avg_price")]
        private decimal AveragePrice2 { get => AveragePrice; set => AveragePrice = value; }
        /// <summary>
        /// Stop price
        /// </summary>
        public decimal StopPrice { get; set; }
        /// <summary>
        /// Limit price
        /// </summary>
        [JsonProperty("limitprice")]
        public decimal Price { get; set; }
        /// <summary>
        /// Miscellaneous info
        /// </summary>
        public string Misc { get; set; } = string.Empty;
        /// <summary>
        /// Order flags
        /// </summary>
        public string Oflags { get; set; } = string.Empty;
        /// <summary>
        /// Reason of failure
        /// </summary>
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// Trade ids
        /// </summary>
        [JsonProperty("trades")]
        public IEnumerable<string> TradeIds { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Order details
    /// </summary>
    public class KrakenOrderInfo
    {
        /// <summary>
        /// The symbol of the order
        /// </summary>
        [JsonProperty("pair")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Side of the order
        /// </summary>
        [JsonProperty("type"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Type of the order
        /// </summary>
        [JsonProperty("ordertype"), JsonConverter(typeof(OrderTypeConverter))]
        public OrderType Type { get; set; }
        /// <summary>
        /// Price of the order
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Secondary price of the order (<see cref="KrakenRestClientSpotApiTrading.PlaceOrderAsync"/> for details)
        /// </summary>
        [JsonProperty("price2")]
        public decimal SecondaryPrice { get; set; }
        /// <summary>
        /// Amount of leverage
        /// </summary>
        public string Leverage { get; set; } = string.Empty;
        /// <summary>
        /// Order description
        /// </summary>
        public string Order { get; set; } = string.Empty;
        /// <summary>
        /// Conditional close order description
        /// </summary>
        public string Close { get; set; } = string.Empty;
    }

    /// <summary>
    /// Stream order update
    /// </summary>
    public class KrakenStreamOrder: KrakenOrder
    {
        /// <summary>
        /// The update sequence number
        /// </summary>
        public int SequenceNumber { get; set; }
    }
}
