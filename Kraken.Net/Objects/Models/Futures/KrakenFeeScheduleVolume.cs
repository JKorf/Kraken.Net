using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFeeScheduleVolumeResult : KrakenFuturesResult<Dictionary<string, decimal>>
    {
        [JsonProperty("volumesByFeeSchedule")]
        public override Dictionary<string, decimal> Data { get; set; } = new Dictionary<string, decimal>();
    }
}
