using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Earn lock type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<LockType>))]
    public enum LockType
    {
        /// <summary>
        /// ["<c>flex</c>"] "Kraken rewards". This is earning on your spot balances where eligible. It's turned on account wide from the UI and you cannot manually allocate to these strategies.
        /// </summary>
        [Map("flex")]
        Flex,
        /// <summary>
        /// ["<c>bonded</c>"] Has an unbonding period. Deallocation will not happen until this period has passed.
        /// </summary>
        [Map("bonded")]
        Bonded,
        /// <summary>
        /// ["<c>timed</c>"] Timed
        /// </summary>
        [Map("timed")]
        Timed,
        /// <summary>
        /// ["<c>instant</c>"] Can be deallocated without an unbonding period. This is called flexible in the UI.
        /// </summary>
        [Map("instant")]
        Instant
    }
}
