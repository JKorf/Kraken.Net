using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesMarginRequirementsInternal : KrakenFuturesResult
    {
        [JsonProperty("initialMargin")]
        public decimal? InitialMargin { get; set; }
        [JsonProperty("price")]
        public decimal? Price { get; set; }
    }

    public class KrakenFuturesMarginRequirements
    {
        [JsonProperty("initialMargin")]
        public decimal? InitialMargin { get; set; }
        [JsonProperty("price")]
        public decimal? Price { get; set; }
    }
}
