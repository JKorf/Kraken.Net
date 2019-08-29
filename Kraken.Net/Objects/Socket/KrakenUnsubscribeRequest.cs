using Newtonsoft.Json;

namespace Kraken.Net.Objects.Socket
{
    internal class KrakenUnsubscribeRequest
    {
        [JsonProperty("event")]
        public string Event { get; set; } = "unsubscribe";
        [JsonProperty("reqid")]
        public int RequestId { get; set; }
        [JsonProperty("channelID")]
        public int ChannelId { get; set; }

        public KrakenUnsubscribeRequest(int requestId, int channelId)
        {
            RequestId = requestId;
            ChannelId = channelId;
        }
    }
}
