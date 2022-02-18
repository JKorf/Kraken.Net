using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// User trade info
    /// </summary>
    public class KrakenUserTrade
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("ordertxid")]
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// Pos id
        /// </summary>
        [JsonProperty("postxid")]
        public string PosId { get; set; } = string.Empty;

        /// <summary>
        /// Trade id
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("pair")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp of trade
        /// </summary>
        [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
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
        [JsonProperty("cost")]
        public decimal QuoteQuantity { get; set; }
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
        public string Misc { get; set; } = string.Empty;

        /// <summary>
        /// Position status
        /// </summary>
        [JsonProperty("posstatus")]
        public string PositionStatus { get; set; } = string.Empty;
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
        public IEnumerable<string> Trades { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Stream trade update
    /// </summary>
    public class KrakenStreamUserTrade: KrakenUserTrade
    {
        /// <summary>
        /// The update sequence number
        /// </summary>
        public int SequenceNumber { get; set; }
    }
}
