using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesSymbolStatusResult : KrakenFuturesResult<IEnumerable<KrakenFuturesSymbolStatus>>
    {
        [JsonProperty("instrumentStatus")]
        public override IEnumerable<KrakenFuturesSymbolStatus> Data { get; set; } = new List<KrakenFuturesSymbolStatus>();
    }

    /// <summary>
    /// Symbol status
    /// </summary>
    public record KrakenFuturesSymbolStatus
    {
        /// <summary>
        /// Extreme volatility initial margin multiplier
        /// </summary>
        public int ExtremeVolatilityInitialMarginMultiplier { get; set; }
        /// <summary>
        /// Is experiencing dislocation
        /// </summary>
        public bool IsExperiencingDislocation { get; set; }
        /// <summary>
        /// Is experiencing exterme volatility
        /// </summary>
        public bool IsExperiencingExtremeVolatility { get; set; }
        /// <summary>
        /// Price dislocation direction
        /// </summary>
        public string? PriceDislocationDirection { get; set; }
        /// <summary>
        /// Tradeable
        /// </summary>
        public string Tradeable { get; set; } = string.Empty;
    }
}
