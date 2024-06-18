﻿using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Tick info
    /// </summary>
    public record KrakenTick
    {
        /// <summary>
        /// High price info
        /// </summary>
        [JsonProperty("h")]
        public KrakenTickInfo High { get; set; } = default!;
        /// <summary>
        /// Low price info
        /// </summary>
        [JsonProperty("l")]
        public KrakenTickInfo Low { get; set; } = default!;
        /// <summary>
        /// Last trade info
        /// </summary>
        [JsonProperty("c")]
        public KrakenLastTrade LastTrade { get; set; } = default!;
        /// <summary>
        /// Best ask info
        /// </summary>
        [JsonProperty("a")]
        public KrakenBestEntry BestAsks { get; set; } = default!;
        /// <summary>
        /// Best bid info
        /// </summary>
        [JsonProperty("b")]
        public KrakenBestEntry BestBids { get; set; } = default!;
        /// <summary>
        /// Trade count info
        /// </summary>
        [JsonProperty("t")]
        public KrakenTickInfo Trades { get; set; } = default!;
        /// <summary>
        /// Volume weighted average price info
        /// </summary>
        [JsonProperty("p")]
        public KrakenTickInfo VolumeWeightedAveragePrice { get; set; } = default!;
        /// <summary>
        /// Volume info
        /// </summary>
        [JsonProperty("v")]
        public KrakenTickInfo Volume { get; set; } = default!;
    }

    /// <summary>
    /// Tick info
    /// </summary>
    public record KrakenRestTick: KrakenTick
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Open price
        /// </summary>
        [JsonProperty("o")]
        public decimal OpenPrice { get; set; }
    }

    /// <summary>
    /// Tick info
    /// </summary>
    public record KrakenStreamTick : KrakenTick
    {
        /// <summary>
        /// Open price info
        /// </summary>
        [JsonProperty("o")]
        public KrakenTickInfo Open { get; set; } = default!;

    }

    /// <summary>
    /// Tick detail info
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public record KrakenTickInfo
    {
        /// <summary>
        /// Value for today
        /// </summary>
        [ArrayProperty(0)]
        public decimal ValueToday { get; set; }
        /// <summary>
        /// Rolling 24h window value
        /// </summary>
        [ArrayProperty(1)]
        public decimal Value24H { get; set; }
    }

    /// <summary>
    /// Last trade details
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public record KrakenLastTrade
    {
        /// <summary>
        /// Price of last trade
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity of last trade
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
    }

    /// <summary>
    /// Best entry info
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public record KrakenBestEntry
    {
        /// <summary>
        /// Price of best entry
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Lot quantity
        /// </summary>
        [ArrayProperty(1)]
        public decimal LotQuantity { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [ArrayProperty(2)]
        public decimal Quantity { get; set; }
    }
}
