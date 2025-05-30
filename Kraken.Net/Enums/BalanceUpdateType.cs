using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Balance update type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<BalanceUpdateType>))]
    public enum BalanceUpdateType
    {
        /// <summary>
        /// Deposit
        /// </summary>
        [Map("deposit")]
        Deposit,
        /// <summary>
        /// Withdrawal
        /// </summary>
        [Map("withdrawal")]
        Withdrawal,
        /// <summary>
        /// Trade
        /// </summary>
        [Map("trade")]
        Trade,
        /// <summary>
        /// Margin
        /// </summary>
        [Map("margin")]
        Margin,
        /// <summary>
        /// Adjustment
        /// </summary>
        [Map("adjustment")]
        Adjustment,
        /// <summary>
        /// Rollover
        /// </summary>
        [Map("rollover")]
        Rollover,
        /// <summary>
        /// Credit
        /// </summary>
        [Map("credit")]
        Credit,
        /// <summary>
        /// Transfer
        /// </summary>
        [Map("transfer")]
        Transfer,
        /// <summary>
        /// Settled
        /// </summary>
        [Map("settled")]
        Settled,
        /// <summary>
        /// Staking
        /// </summary>
        [Map("staking")]
        Staking,
        /// <summary>
        /// Sale
        /// </summary>
        [Map("sale")]
        Sale,
        /// <summary>
        /// Reserve
        /// </summary>
        [Map("reserve")]
        Reserve,
        /// <summary>
        /// Conversion
        /// </summary>
        [Map("conversion")]
        Conversion,
        /// <summary>
        /// Dividend
        /// </summary>
        [Map("dividend")]
        Dividend,
        /// <summary>
        /// Reward
        /// </summary>
        [Map("reward")]
        Reward,
        /// <summary>
        /// Creator fee
        /// </summary>
        [Map("creator_fee")]
        CreatorFee,
    }

}
