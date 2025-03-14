using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesPositionResult : KrakenFuturesResult<KrakenFuturesPosition[]>
    {
        [JsonPropertyName("openPositions")]
        public override KrakenFuturesPosition[] Data { get; set; } = Array.Empty<KrakenFuturesPosition>();
    }

    /// <summary>
    /// Futures position info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesPosition
    {
        /// <summary>
        /// Position enter time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("fillTime")]
        public DateTime FillTime { get; set; }
        /// <summary>
        /// Max leverage selected for isolated position
        /// </summary>
        [JsonPropertyName("maxFixedLeverage")]
        public decimal? MaxFixedLeverage { get; set; }
        /// <summary>
        /// Selected pnl currency for the position (default: USD)
        /// </summary>
        [JsonPropertyName("pnlCurrency")]
        public string? ProfitAndLossCurrency { get; set; }
        /// <summary>
        /// The average price at which the position was entered into.
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// The direction of the position.
        /// </summary>

        [JsonPropertyName("side")]
        public PositionSide Side { get; set; }
        /// <summary>
        /// The size of the position.
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Unrealised funding on the position.
        /// </summary>
        [JsonPropertyName("unrealizedFunding")]
        public decimal? UnrealizedFunding { get; set; }
    }
}
