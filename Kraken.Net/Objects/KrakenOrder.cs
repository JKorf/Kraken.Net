using System;
using System.Collections.Generic;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Kraken.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Order info
    /// </summary>
    public class KrakenOrder: ICommonOrder
    {
        /// <summary>
        /// Reference id
        /// </summary>
        [JsonProperty("refid")]
        public string ReferenceId { get; set; } = "";
        /// <summary>
        /// Client reference id
        /// </summary>
        [JsonProperty("userref")]
        public string ClientOrderId { get; set; } = "";
        /// <summary>
        /// Status of the order
        /// </summary>
        [JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Open timestamp
        /// </summary>
        [JsonProperty("opentm"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Start timestamp
        /// </summary>
        [JsonProperty("starttm"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Expire timestamp
        /// </summary>
        [JsonProperty("expiretm"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// Close timestamp
        /// </summary>
        [JsonProperty("closetm"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime? ClosedTime { get; set; }

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
        public decimal ExecutedQuantity { get; set; }
        /// <summary>
        /// Cost of the order
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// Average price of the order
        /// </summary>
        [JsonProperty("price")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        public decimal StopPrice { get; set; }
        /// <summary>
        /// Limit price
        /// </summary>
        public decimal LimitPrice { get; set; }
        /// <summary>
        /// Miscellaneous info
        /// </summary>
        public string Misc { get; set; } = "";
        /// <summary>
        /// Order flags
        /// </summary>
        public string Oflags { get; set; } = "";
        /// <summary>
        /// Reason of failure
        /// </summary>
        [JsonOptionalProperty]
        public string Reason { get; set; } = "";
        /// <summary>
        /// Trade ids
        /// </summary>
        [JsonProperty("trades")]
        [JsonOptionalProperty]
        public IEnumerable<string> TradeIds { get; set; } = new List<string>();

        string ICommonOrderId.CommonId => ReferenceId;
        string ICommonOrder.CommonSymbol => OrderDetails.Symbol;
        decimal ICommonOrder.CommonPrice => OrderDetails.Price;
        decimal ICommonOrder.CommonQuantity => Quantity;
        string ICommonOrder.CommonStatus => Status.ToString();
        bool ICommonOrder.IsActive => Status == OrderStatus.Open;

        IExchangeClient.OrderSide ICommonOrder.CommonSide => OrderDetails.Side == OrderSide.Sell
            ? IExchangeClient.OrderSide.Sell
            : IExchangeClient.OrderSide.Buy;

        IExchangeClient.OrderType ICommonOrder.CommonType
        {
            get
            {
                if (OrderDetails.Type == OrderType.Limit) return IExchangeClient.OrderType.Limit;
                if (OrderDetails.Type == OrderType.Market) return IExchangeClient.OrderType.Market;
                return IExchangeClient.OrderType.Other;
            }
        }
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
        public string Symbol { get; set; } = "";
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
        /// Secondary price of the order (<see cref="KrakenClient.PlaceOrder"/> for details)
        /// </summary>
        [JsonProperty("price2")]
        public decimal SecondaryPrice { get; set; }
        /// <summary>
        /// Amount of leverage
        /// </summary>
        public string Leverage { get; set; } = "";
        /// <summary>
        /// Order description
        /// </summary>
        public string Order { get; set; } = "";
        /// <summary>
        /// Conditional close order description
        /// </summary>
        public string Close { get; set; } = "";
    }
}
