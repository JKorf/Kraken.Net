using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Objects.Options
{
    /// <summary>
    /// Kraken options
    /// </summary>
    public class KrakenOptions : LibraryOptions<KrakenRestOptions, KrakenSocketOptions, KrakenCredentials, KrakenEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}
