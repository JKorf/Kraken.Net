using Newtonsoft.Json;

namespace Kraken.Net.Objects.Socket
{
    internal class KrakenSubscribeRequest
    {
        [JsonProperty("event")]
        public string Event { get; set; } = "subscribe";
        [JsonProperty("reqid")]
        public int RequestId { get; set; }
        [JsonProperty("pair")]
        public string[] Markets { get; set; }
        [JsonProperty("subscription")]
        public KrakenSubscriptionDetails Details { get; set; }

        [JsonIgnore]
        public int ChannelId { get; set; }

        public KrakenSubscribeRequest(string topic,  int requestId, params string[] markets)
        {
            RequestId = requestId;
            Markets = markets;
            Details = new KrakenSubscriptionDetails(topic);
        }
    }

    internal class KrakenSubscriptionDetails
    {
        [JsonProperty("name")]
        public string Topic { get; set; }

        public KrakenSubscriptionDetails(string topic)
        {
            Topic = topic;
        }
    }

    internal class KrakenOHLCSubscriptionDetails: KrakenSubscriptionDetails
    {
        [JsonProperty("interval")]
        public int Interval { get; set; }

        public KrakenOHLCSubscriptionDetails(int interval) : base("ohlc")
        {
            Interval = interval;
        }
    }

    internal class KrakenDepthSubscriptionDetails : KrakenSubscriptionDetails
    {
        [JsonProperty("depth")]
        public int Depth { get; set; }

        public KrakenDepthSubscriptionDetails(int depth) : base("book")
        {
            Depth = depth;
        }
    }
}
