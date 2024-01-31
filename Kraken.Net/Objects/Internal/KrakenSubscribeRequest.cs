using System.Linq;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSubscribeRequest : KrakenSocketRequest
    {
        [JsonProperty("pair", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[]? Symbols { get; set; }
        [JsonProperty("subscription")]
        public KrakenSubscriptionDetails Details { get; set; }

        public KrakenSubscribeRequest(string topic, string? token, int? interval, bool? snapshot, int? depth, int requestId, params string[]? symbols)
        {
            RequestId = requestId;
            Details = new KrakenSubscriptionDetails(topic, token, interval, snapshot, depth);
            if(symbols?.Any() == true)
                Symbols = symbols;
        }
    }


    internal class KrakenSubscriptionDetails
    {
        [JsonProperty("name")]
        public string Topic { get; set; }

        [JsonProperty("token", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Token { get; set; }

        [JsonProperty("interval", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Interval { get; set; }

        [JsonProperty("snapshot", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Snapshot { get; set; }

        [JsonProperty("depth", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Depth { get; set; }

        public KrakenSubscriptionDetails(string topic, string? token, int? interval, bool? snapshot, int? depth)
        {
            Topic = topic;
            Token = token;
            Interval = interval;
            Snapshot = snapshot;
            Depth = depth;
        }
    }
}
