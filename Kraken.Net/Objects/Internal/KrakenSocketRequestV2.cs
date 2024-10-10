using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketRequestV2<T>
    {
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;
        [JsonPropertyName("params")]
        public T Parameters { get; set; } = default!;
        [JsonPropertyName("req_id")]
        public long RequestId { get; set; }
    }
}
