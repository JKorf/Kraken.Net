using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketSubRequest
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonPropertyName("symbol"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string[]? Symbol { get; set; }

        [JsonPropertyName("interval"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? Interval { get; set; }

        [JsonPropertyName("snapshot"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? Snapshot { get; set; }

        [JsonPropertyName("snap_orders"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? SnapshotOrders { get; set; }

        [JsonPropertyName("snap_trades"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? SnapshotTrades { get; set; }

        [JsonPropertyName("token"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Token { get; set; }
    }
}
