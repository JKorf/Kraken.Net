namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Response info
    /// </summary>
    public class KrakenSocketResponseBase
    {
        /// <summary>
        /// Event type
        /// </summary>
        public string Event { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// Error message if not successful
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
