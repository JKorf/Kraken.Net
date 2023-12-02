using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesMaxOrderSizeInternal : KrakenFuturesResult
    {
        [JsonProperty("buyPrice")]
        public decimal? BuyPrice { get; set; }
        [JsonProperty("maxBuySize")]
        public decimal? MaxBuyQuantity { get; set; }
        [JsonProperty("maxSellSize")]
        public decimal? MaxSellQuantity { get; set; }
        [JsonProperty("sellPrice")]
        public decimal? SellPrice { get; set; }
    }

    /// <summary>
    /// Max order size
    /// </summary>
    public class KrakenFuturesMaxOrderSize
    {
        /// <summary>
        /// Buy price
        /// </summary>
        [JsonProperty("buyPrice")]
        public decimal? BuyPrice { get; set; }
        /// <summary>
        /// Max buy quantity
        /// </summary>
        [JsonProperty("maxBuySize")]
        public decimal? MaxBuyQuantity { get; set; }
        /// <summary>
        /// Max sell quantity
        /// </summary>
        [JsonProperty("maxSellSize")]
        public decimal? MaxSellQuantity { get; set; }
        /// <summary>
        /// Sell price
        /// </summary>
        [JsonProperty("sellPrice")]
        public decimal? SellPrice { get; set; }
    }
}
