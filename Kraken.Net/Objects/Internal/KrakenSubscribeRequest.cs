//namespace Kraken.Net.Objects.Internal
//{
//    internal class KrakenSubscribeRequest : KrakenSocketRequest
//    {
//        [JsonPropertyName("pair"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
//        public string[]? Symbols { get; set; }
//        [JsonPropertyName("subscription")]
//        public KrakenSubscriptionDetails Details { get; set; }

//        public KrakenSubscribeRequest(string topic, string? token, int? interval, bool? snapshot, int? depth, int requestId, params string[]? symbols)
//        {
//            RequestId = requestId;
//            Details = new KrakenSubscriptionDetails(topic, token, interval, snapshot, depth);
//            if(symbols?.Any() == true)
//                Symbols = symbols;
//        }
//    }


//    internal class KrakenSubscriptionDetails
//    {
//        [JsonPropertyName("name")]
//        public string Topic { get; set; }

//        [JsonPropertyName("token"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
//        public string? Token { get; set; }

//        [JsonPropertyName("interval"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
//        public int? Interval { get; set; }

//        [JsonPropertyName("snapshot"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
//        public bool? Snapshot { get; set; }

//        [JsonPropertyName("depth"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
//        public int? Depth { get; set; }

//        public KrakenSubscriptionDetails(string topic, string? token, int? interval, bool? snapshot, int? depth)
//        {
//            Topic = topic;
//            Token = token;
//            Interval = interval;
//            Snapshot = snapshot;
//            Depth = depth;
//        }
//    }
//}
