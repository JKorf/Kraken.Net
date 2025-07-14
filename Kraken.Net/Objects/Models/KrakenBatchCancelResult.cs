using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Cancel result
    /// </summary>
    public record KrakenBatchCancelResult
    {
        /// <summary>
        /// Canceled count
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
