namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesMaxOrderSizeInternal : KrakenFuturesResult
    {
        [JsonPropertyName("buyPrice")]
        public decimal? BuyPrice { get; set; }
        [JsonPropertyName("maxBuySize")]
        public decimal? MaxBuyQuantity { get; set; }
        [JsonPropertyName("maxSellSize")]
        public decimal? MaxSellQuantity { get; set; }
        [JsonPropertyName("sellPrice")]
        public decimal? SellPrice { get; set; }
    }

    /// <summary>
    /// Max order size
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesMaxOrderSize
    {
        /// <summary>
        /// Buy price
        /// </summary>
        [JsonPropertyName("buyPrice")]
        public decimal? BuyPrice { get; set; }
        /// <summary>
        /// Max buy quantity
        /// </summary>
        [JsonPropertyName("maxBuySize")]
        public decimal? MaxBuyQuantity { get; set; }
        /// <summary>
        /// Max sell quantity
        /// </summary>
        [JsonPropertyName("maxSellSize")]
        public decimal? MaxSellQuantity { get; set; }
        /// <summary>
        /// Sell price
        /// </summary>
        [JsonPropertyName("sellPrice")]
        public decimal? SellPrice { get; set; }
    }
}
