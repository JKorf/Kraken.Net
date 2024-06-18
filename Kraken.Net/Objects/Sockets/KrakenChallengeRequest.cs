using Newtonsoft.Json;

namespace Kraken.Net.Objects.Sockets
{
    internal record KrakenChallengeRequest : KrakenEvent
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; } = string.Empty;
    }

    internal record KrakenChallengeResponse : KrakenEvent
    {
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }
}
