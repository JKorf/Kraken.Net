using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenOrderHistoryResult : KrakenFuturesResult<KrakenOrderHistory[]>
    {
        [JsonPropertyName("elements")]
        public override KrakenOrderHistory[] Data { get; set; } = Array.Empty<KrakenOrderHistory>();
    }

    /// <summary>
    /// Order history
    /// </summary>
    public record KrakenOrderHistory
    {
    }
}
