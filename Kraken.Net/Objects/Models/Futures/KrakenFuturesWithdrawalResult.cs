namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesWithdrawalResult : KrakenFuturesResult<string>
    {
        [JsonPropertyName("uid")]
        public override string Data { get; set; } = string.Empty;
    }
}
