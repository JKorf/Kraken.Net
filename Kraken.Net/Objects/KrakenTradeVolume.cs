using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Trade volume info
    /// </summary>
    public class KrakenTradeVolume
    {
        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; } = "";
        /// <summary>
        /// Volume
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Fees structure
        /// </summary>
        public IEnumerable<KrakenFeeStruct> Fees { get; set; } = new List<KrakenFeeStruct>();
        /// <summary>
        /// Maker fees structure
        /// </summary>
        [JsonProperty("fees_maker")]
        public IEnumerable<KrakenFeeStruct> MakerFees { get; set; } = new List<KrakenFeeStruct>();
    }

    /// <summary>
    /// Fee level info
    /// </summary>
    public class KrakenFeeStruct
    {
        /// <summary>
        /// Fee
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// Minimal fee
        /// </summary>
        [JsonProperty("minfee")]
        public decimal MinimalFee { get; set; }
        /// <summary>
        /// Maximal fee
        /// </summary>
        [JsonProperty("maxfee")]
        public decimal MaximumFee { get; set; }
        /// <summary>
        /// Next fee
        /// </summary>
        public decimal NextFee { get; set; }
        /// <summary>
        /// Next volume
        /// </summary>
        public decimal NextVolume { get; set; }
        /// <summary>
        /// Tier volume
        /// </summary>
        public decimal TierVolume { get; set; }
    }
}
