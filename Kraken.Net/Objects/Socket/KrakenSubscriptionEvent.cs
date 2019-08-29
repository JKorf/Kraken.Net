using Newtonsoft.Json;

namespace Kraken.Net.Objects.Socket
{
    internal class KrakenSubscriptionEvent
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string Event { get; set; }
        public string Pair { get; set; }
        public string Status { get; set; }
        [JsonProperty("reqid")]
        public string RequestId { get; set; }
        public KrakenSubscriptionDetails Subscription { get; set; }
        public string ErrorMessage { get; set; }
    }
}
