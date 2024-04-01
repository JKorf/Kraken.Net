using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Earn status
    /// </summary>
    public class KrakenEarnStatus
    {
        /// <summary>
        /// Is pending
        /// </summary>
        [JsonProperty("pending")]
        public bool Pending { get; set; }
    }
}
