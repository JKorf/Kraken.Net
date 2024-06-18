﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Sockets
{
    internal record KrakenSubscriptionEvent : KrakenQueryEvent
    {
        [JsonProperty("channelID")]
        public long ChannelId { get; set; }
        [JsonProperty("channelName")]
        public string ChannelName { get; set; } = string.Empty;
        [JsonProperty("pair")]
        public string Symbol { get; set; } = string.Empty;
        [JsonProperty("reqId")]
        public int RequestId { get; set; }
        [JsonProperty("subscription")]
        public Dictionary<string, string> Subscription { get; set; } = new Dictionary<string, string>();
    }
}
