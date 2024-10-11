using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketAuthRequestV2
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
