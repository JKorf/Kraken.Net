namespace Kraken.Net.Objects.Sockets
{
    internal record KrakenChallengeRequest
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; } = string.Empty;
    }

    internal record KrakenChallengeResponse : KrakenEvent
    {
        [JsonPropertyName("event")]
        public string? Event { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
