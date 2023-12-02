using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models
{
    public class KrakenBatchOrderResult
    {
        [JsonProperty("orders")]
        public IEnumerable<KrakenPlacedBatchOrder> Orders { get; set; }
    }

    public class KrakenPlacedBatchOrder
    {
        [JsonProperty("txid")]
        public string OrderId { get; set; }
        [JsonProperty("descr")]
        public KrakenPlacedOrderDescription Description { get; set; }
    }
}
