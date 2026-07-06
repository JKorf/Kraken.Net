using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Trade volume symbol request
    /// </summary>
    public record TradeVolumeRequest
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("asset")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Asset class
        /// </summary>
        [JsonPropertyName("aclass")]
        public AssetClassExtended AssetClass { get; set; } 
    }
}
