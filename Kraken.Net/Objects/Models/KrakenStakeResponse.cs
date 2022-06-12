namespace Kraken.Net.Objects.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Kraken's response to a staking request.
    /// </summary>
    public class KrakenStakeResponse
    {
        /// <summary>
        /// Reference id which can be tracked back to a ledger entry corresponding to the
        /// staked asset (e.g. DOT.S).
        /// </summary>
        [JsonProperty("refid")]
        public string ReferenceId { get; set; } = null!;
    }
}
