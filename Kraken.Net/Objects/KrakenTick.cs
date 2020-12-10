using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Tick info
    /// </summary>
    public class KrakenTick
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
    public class KrakenRestTick: KrakenTick, ICommonTicker
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = "";
        /// <summary>
        /// Open price
        /// </summary>
        [JsonProperty("o")]
        public decimal Open { get; set; }

        string ICommonTicker.CommonSymbol => Symbol;
        decimal ICommonTicker.CommonHigh => High.Value24H;
        decimal ICommonTicker.CommonLow => Low.Value24H;
        decimal ICommonTicker.CommonVolume => Volume.Value24H;
    }

    /// <summary>
    /// Tick info
    /// </summary>
    public class KrakenStreamTick : KrakenTick
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
    public class KrakenTickInfo
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
    public class KrakenLastTrade
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
    public class KrakenBestEntry
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
