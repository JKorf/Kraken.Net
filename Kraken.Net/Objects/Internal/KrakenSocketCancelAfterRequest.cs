using Newtonsoft.Json;

namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Place order request
    /// </summary>
    internal class KrakenSocketCancelAfterRequest : KrakenSocketRequestBase
    {
        /// <summary>
        /// Timeout
        /// </summary>
        [JsonProperty("timeout")]
        public int Timeout { get; set; }
    }
}
