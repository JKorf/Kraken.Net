using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Earn lock type
    /// </summary>
    public enum LockType
    {
        /// <summary>
        /// "Kraken rewards". This is earning on your spot balances where eligible. It's turned on account wide from the UI and you cannot manually allocate to these strategies.
        /// </summary>
        [Map("flex")]
        Flex,
        /// <summary>
        /// Has an unbonding period. Deallocation will not happen until this period has passed.
        /// </summary>
        [Map("bonded")]
        Bonded,
        /// <summary>
        /// Timed
        /// </summary>
        [Map("timed")]
        Timed,
        /// <summary>
        /// Can be deallocated without an unbonding period. This is called flexible in the UI.
        /// </summary>
        [Map("instant")]
        Instant
    }
}
