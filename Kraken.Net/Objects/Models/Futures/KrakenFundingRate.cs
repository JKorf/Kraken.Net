using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFundingRatesResult : KrakenFuturesResult<KrakenFundingRate[]>
    {
        [JsonPropertyName("rates")]
        public override KrakenFundingRate[] Data { get; set; } = Array.Empty<KrakenFundingRate>();
    }

    /// <summary>
    /// Funding rate info
    /// </summary>
    [SerializationModel]
    public record KrakenFundingRate
    {
        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal FundingRate { get; set; }
        /// <summary>
        /// Relative funding rate
        /// </summary>
        [JsonPropertyName("relativeFundingRate")]
        public decimal RelativeFundingRate { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
