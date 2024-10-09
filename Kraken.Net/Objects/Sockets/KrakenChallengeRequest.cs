namespace Kraken.Net.Objects.Sockets
{
    internal record KrakenChallengeRequest : KrakenEvent
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; } = string.Empty;
    }

    internal record KrakenChallengeResponse : KrakenEvent
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
