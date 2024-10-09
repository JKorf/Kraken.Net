namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFeeScheduleVolumeResult : KrakenFuturesResult<Dictionary<string, decimal>>
    {
        [JsonPropertyName("volumesByFeeSchedule")]
        public override Dictionary<string, decimal> Data { get; set; } = new Dictionary<string, decimal>();
    }
}
