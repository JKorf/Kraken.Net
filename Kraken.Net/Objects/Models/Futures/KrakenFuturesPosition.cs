using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesPositionResult : KrakenFuturesResult<IEnumerable<KrakenFuturesPosition>>
    {
        [JsonProperty("openPositions")]
        public override IEnumerable<KrakenFuturesPosition> Data { get; set; } = Array.Empty<KrakenFuturesPosition>();
    }

    /// <summary>
    /// Futures position info
    /// </summary>
    public record KrakenFuturesPosition
    {
        /// <summary>
        /// Position enter time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime FillTime { get; set; }
        /// <summary>
        /// Max leverage selected for isolated position
        /// </summary>
        public decimal? MaxFixedLeverage { get; set; }
        /// <summary>
        /// Selected pnl currency for the position (default: USD)
        /// </summary>
        public string? ProfitAndLossCurrency { get; set; }
        /// <summary>
        /// The average price at which the position was entered into.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The direction of the position.
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public PositionSide Side { get; set; }
        /// <summary>
        /// The size of the position.
        /// </summary>
        [JsonProperty("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The profit and loss currency
        /// </summary>
        [JsonProperty("pnlCurrency")]
        public string? PnlCurrency { get; set; }
        /// <summary>
        /// Unrealised funding on the position.
        /// </summary>
        public decimal? UnrealizedFunding { get; set; }
    }
}
