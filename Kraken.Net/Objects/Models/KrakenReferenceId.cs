using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Id
    /// </summary>
    public class KrakenReferenceId
    {
        /// <summary>
        /// The id
        /// </summary>
        [JsonProperty("refid")]
        public string ReferenceId { get; set; } = string.Empty;
    }
}
