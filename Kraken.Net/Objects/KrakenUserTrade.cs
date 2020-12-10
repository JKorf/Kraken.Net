using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Kraken.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// User trade info
    /// </summary>
    public class KrakenUserTrade: ICommonTrade
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("ordertxid")]
        public string OrderId { get; set; } = "";

        /// <summary>
        /// Trade id
        /// </summary>
        [JsonIgnore]
        public string TradeId { get; set; }

        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("pair")]
        public string Symbol { get; set; } = "";
        /// <summary>
        /// Timestamp of trade
        /// </summary>
        [JsonProperty("time"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonProperty("type"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonProperty("ordertype"), JsonConverter(typeof(OrderTypeConverter))]
        public OrderType Type { get; set; }
        /// <summary>
        /// Price of the trade
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Cost of the trade
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Fee paid for trade
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// Quantity of the trade
        /// </summary>
        [JsonProperty("vol")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Margin
        /// </summary>
        public decimal Margin { get; set; }

        /// <summary>
        /// Misc info
        /// </summary>
        public string Misc { get; set; } = "";

        /// <summary>
        /// Position status
        /// </summary>
        [JsonProperty("posstatus")]
        public string PositionStatus { get; set; } = "";
        /// <summary>
        /// Closed average price
        /// </summary>
        [JsonProperty("cprice")]
        public decimal? ClosedAveragePrice { get; set; }
        /// <summary>
        /// Closed cost
        /// </summary>
        [JsonProperty("ccost")]
        public decimal? ClosedCost { get; set; }
        /// <summary>
        /// Closed fee
        /// </summary>
        [JsonProperty("cfee")]
        public decimal? ClosedFee { get; set; }
        /// <summary>
        /// Closed quantity
        /// </summary>
        [JsonProperty("cvol")]
        public decimal? ClosedQuantity { get; set; }
        /// <summary>
        /// Closed margin
        /// </summary>
        [JsonProperty("cmargin")]
        public decimal? ClosedMargin { get; set; }
        /// <summary>
        /// Closed net profit/loss
        /// </summary>
        [JsonProperty("net")]
        public decimal? ClosedProfitLoss { get; set; }
        /// <summary>
        /// Trade ids
        /// </summary>
        public IEnumerable<string> Trades { get; set; } = new List<string>();

        string ICommonTrade.CommonId => TradeId;
        decimal ICommonTrade.CommonPrice => Price;
        decimal ICommonTrade.CommonQuantity => Quantity;
        decimal ICommonTrade.CommonFee => Fee;
        string? ICommonTrade.CommonFeeAsset => null;
    }
}
