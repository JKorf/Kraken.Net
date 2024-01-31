using Newtonsoft.Json;

namespace Kraken.Net.Objects.Sockets
{
    internal class KrakenChallengeRequest : KrakenEvent
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; } = string.Empty;
    }

    internal class KrakenChallengeResponse : KrakenEvent
    {
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }
}
