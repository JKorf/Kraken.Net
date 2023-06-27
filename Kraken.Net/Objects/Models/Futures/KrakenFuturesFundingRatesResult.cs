using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    public class KrakenFuturesFundingRatesResult : KrakenFuturesResult<IEnumerable<KrakenFundingRate>>
    {
        [JsonProperty("rates")]
        public override IEnumerable<KrakenFundingRate> Data { get; set; }
    }
}
