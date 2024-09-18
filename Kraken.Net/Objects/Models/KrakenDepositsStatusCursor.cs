namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Deposits status info with cursor
    /// </summary>
    public record KrakenDepositsStatusCursor 
    {
        [JsonProperty("deposits")]
        public IEnumerable<KrakenMovementStatus>? Deposits {  get; set; }

        [JsonProperty("next_cursor")]
        public string? NextCursor { get; set; }
    }
}
