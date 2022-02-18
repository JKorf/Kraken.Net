using Newtonsoft.Json;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSubscriptionEvent
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public string Event { get; set; } = string.Empty;
        public string Pair { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        [JsonProperty("reqid")]
        public string RequestId { get; set; } = string.Empty;
        public KrakenSubscriptionDetails? Subscription { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
