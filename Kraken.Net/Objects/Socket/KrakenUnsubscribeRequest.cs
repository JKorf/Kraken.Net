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
        public int? ChannelId { get; set; }
        [JsonProperty("subscription")]
        public KrakenUnsubscribeSubscription? Subscription { get; set; }

        public KrakenUnsubscribeRequest(int requestId, int channelId)
        {
            RequestId = requestId;
            ChannelId = channelId;
        }

        public KrakenUnsubscribeRequest(int requestId, KrakenUnsubscribeSubscription sub)
        {
            RequestId = requestId;
            Subscription = sub;
        }
    }

    internal class KrakenUnsubscribeSubscription
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
