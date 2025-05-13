using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;
namespace Kraken.Net.Enums
{
    /// <summary>
    /// Tier to use for ratelimiting
    /// </summary>
    [JsonConverter(typeof(EnumConverter<RateLimitTier>))]
    public enum RateLimitTier
    {
        /// <summary>
        /// Starter/default
        /// </summary>
        Starter,
        /// <summary>
        /// Intermediate
        /// </summary>
        Intermediate,
        /// <summary>
        /// PRo
        /// </summary>
        Pro
    }
}
