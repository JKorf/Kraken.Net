namespace Kraken.Net.Objects.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Kraken's response to an unstaking request.
    /// </summary>
    public class KrakenUnstakeResponse
    {
        /// <summary>
        /// Reference id which can be tracked back to a ledger entry corresponding to the
        /// unstaked asset (e.g. DOT.S).
        /// </summary>
        [JsonProperty("refid")]
        public string ReferenceId { get; set; } = null!;
    }
}
