using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesUserTradeResult : KrakenFuturesResult<IEnumerable<KrakenFuturesUserTrade>>
    {
        [JsonProperty("fills")]
        public override IEnumerable<KrakenFuturesUserTrade> Data { get; set; } = new List<KrakenFuturesUserTrade>();
    }

    /// <summary>
    /// User trade info
    /// </summary>
    public record KrakenFuturesUserTrade
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonProperty("cliOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Time of the trade
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime FillTime { get; set; }
        /// <summary>
        /// Type of trade
        /// </summary>
        [JsonProperty("fillType")]
        [JsonConverter(typeof(EnumConverter))]
        public TradeType Type { get; set; }
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonProperty("fill_id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
    }
}
