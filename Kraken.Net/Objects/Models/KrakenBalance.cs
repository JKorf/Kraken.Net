namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    public record KrakenBalance
    {
        /// <summary>
        /// Asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Balance
        /// </summary>
        public decimal Balance { get; set; }
    }
}
