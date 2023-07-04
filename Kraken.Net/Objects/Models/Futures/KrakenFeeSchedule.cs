using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFeeSchedulesResult : KrakenFuturesResult<IEnumerable<KrakenFeeSchedule>>
    {
        [JsonProperty("feeSchedules")]
        public override IEnumerable<KrakenFeeSchedule> Data { get; set; } = new List<KrakenFeeSchedule>();
    }

    /// <summary>
    /// Fee info
    /// </summary>
    public class KrakenFeeSchedule
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Id
        /// </summary>
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// Fee tiers
        /// </summary>
        public IEnumerable<KrakenFee> Tiers { get; set; } = new List<KrakenFee>();
    }

    /// <summary>
    /// Fee info when trading volume is above specific volume
    /// </summary>
    public class KrakenFee
    {
        /// <summary>
        /// Fee for maker orders
        /// </summary>
        public decimal MakerFee { get; set; }
        /// <summary>
        /// Fee for taker orders
        /// </summary>
        public decimal TakerFee { get; set; }
        /// <summary>
        /// Usd trade volume threshold
        /// </summary>
        public decimal UsdVolume { get; set; }
    }
}
