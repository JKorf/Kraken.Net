using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info about a withdraw method
    /// </summary>
    public class KrakenWithdrawMethod
    {
        /// <summary>
        /// Name of the asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Name of the method
        /// </summary>
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// Name of the Network
        /// </summary>
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// Minimum amount
        /// </summary>
        public decimal Minimum { get; set; }
    }
}
