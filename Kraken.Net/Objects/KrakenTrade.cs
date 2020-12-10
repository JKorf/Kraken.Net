using System;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Kraken.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Trade info
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenTrade: ICommonRecentTrade
    {
        /// <summary>
        /// Price of the trade
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity of the trade
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp of trade
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [ArrayProperty(3), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [ArrayProperty(4), JsonConverter(typeof(OrderTypeMinimalConverter))]
        public OrderTypeMinimal Type { get; set; }

        /// <summary>
        /// Misc info
        /// </summary>
        [ArrayProperty(5)]
        public string Misc { get; set; } = "";

        decimal ICommonRecentTrade.CommonPrice => Price;
        decimal ICommonRecentTrade.CommonQuantity => Quantity;
        DateTime ICommonRecentTrade.CommonTradeTime => Timestamp;
    }
}
