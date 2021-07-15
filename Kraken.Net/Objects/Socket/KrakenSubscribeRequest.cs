using System.Linq;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Socket
{
    internal class KrakenSubscribeRequest
    {
        [JsonProperty("event")]
        public string Event { get; set; } = "subscribe";
        [JsonProperty("reqid")]
        public int RequestId { get; set; }
        [JsonProperty("pair", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[]? Symbols { get; set; }
        [JsonProperty("subscription")]
        public KrakenSubscriptionDetails Details { get; set; }

        [JsonIgnore]
        public int? ChannelId { get; set; }

        public KrakenSubscribeRequest(string topic, int requestId, params string[] symbols)
        {
            RequestId = requestId;
            Details = new KrakenSubscriptionDetails(topic);
            if(symbols.Any())
                Symbols = symbols;
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

    internal class KrakenOpenOrdersSubscriptionDetails : KrakenSubscriptionDetails
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        public KrakenOpenOrdersSubscriptionDetails(string token) : base("openOrders")
        {
            Token = token;
        }
    }

    internal class KrakenOwnTradesSubscriptionDetails : KrakenSubscriptionDetails
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        public KrakenOwnTradesSubscriptionDetails(string token) : base("ownTrades")
        {
            Token = token;
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
