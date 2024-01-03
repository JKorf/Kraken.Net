using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info about a withdraw address
    /// </summary>
    public class KrakenWithdrawAddress
    {
        /// <summary>
        /// The actual address
        /// </summary>
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Name of the asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Name of the method
        /// </summary>
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; } = string.Empty;
        /// <summary>
        /// Verified indicator
        /// </summary>
        public bool Verified { get; set; }
    }
}
