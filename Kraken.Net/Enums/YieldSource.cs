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
        /// ["<c>staking</c>"] Staking
        /// </summary>
        [Map("staking")]
        Staking,
        /// <summary>
        /// ["<c>staking</c>"] Off chain
        /// </summary>
        [Map("staking")]
        OffChain,
        /// <summary>
        /// ["<c>opt_in_rewards</c>"] Opt in rewards
        /// </summary>
        [Map("opt_in_rewards")]
        OptInRewards
    }
}
