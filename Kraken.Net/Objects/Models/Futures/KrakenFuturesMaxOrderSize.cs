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

    public class KrakenFuturesMaxOrderSize
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
}
