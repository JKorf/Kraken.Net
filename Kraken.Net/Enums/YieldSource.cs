using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Yield source
    /// </summary>
    [JsonConverter(typeof(EnumConverter<YieldSource>))]
    public enum YieldSource
    {
        /// <summary>
        /// Staking
        /// </summary>
        [Map("staking")]
        Staking,
        /// <summary>
        /// Off chain
        /// </summary>
        [Map("staking")]
        OffChain,
        /// <summary>
        /// Opt in rewards
        /// </summary>
        [Map("opt_in_rewards")]
        OptInRewards
    }
}
