﻿namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesMarginRequirementsInternal : KrakenFuturesResult
    {
        [JsonPropertyName("initialMargin")]
        public decimal? InitialMargin { get; set; }
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
    }

    /// <summary>
    /// Minimal margin requirements
    /// </summary>
    public record KrakenFuturesMarginRequirements
    {
        /// <summary>
        /// Initial margin
        /// </summary>
        [JsonPropertyName("initialMargin")]
        public decimal? InitialMargin { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
    }
}
