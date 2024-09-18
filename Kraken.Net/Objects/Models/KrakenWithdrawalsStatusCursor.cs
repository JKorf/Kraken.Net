namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Withdrawals status info with cursor
    /// </summary>
    public record KrakenWithdrawalsStatusCursor
    {
        [JsonProperty("withdrawals")]
        public IEnumerable<KrakenMovementStatus>? Withdrawals { get; set; }

        [JsonProperty("next_cursor")]
        public string? NextCursor { get; set; }
    }
}
