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
        /// ["<c>deposit</c>"] Deposit
        /// </summary>
        [Map("deposit")]
        Deposit,
        /// <summary>
        /// ["<c>withdrawal</c>"] Withdrawal
        /// </summary>
        [Map("withdrawal")]
        Withdrawal,
        /// <summary>
        /// ["<c>trade</c>"] Trade
        /// </summary>
        [Map("trade")]
        Trade,
        /// <summary>
        /// ["<c>margin</c>"] Margin
        /// </summary>
        [Map("margin")]
        Margin,
        /// <summary>
        /// ["<c>adjustment</c>"] Adjustment
        /// </summary>
        [Map("adjustment")]
        Adjustment,
        /// <summary>
        /// ["<c>rollover</c>"] Rollover
        /// </summary>
        [Map("rollover")]
        Rollover,
        /// <summary>
        /// ["<c>credit</c>"] Credit
        /// </summary>
        [Map("credit")]
        Credit,
        /// <summary>
        /// ["<c>transfer</c>"] Transfer
        /// </summary>
        [Map("transfer")]
        Transfer,
        /// <summary>
        /// ["<c>settled</c>"] Settled
        /// </summary>
        [Map("settled")]
        Settled,
        /// <summary>
        /// ["<c>staking</c>"] Staking
        /// </summary>
        [Map("staking")]
        Staking,
        /// <summary>
        /// ["<c>sale</c>"] Sale
        /// </summary>
        [Map("sale")]
        Sale,
        /// <summary>
        /// ["<c>reserve</c>"] Reserve
        /// </summary>
        [Map("reserve")]
        Reserve,
        /// <summary>
        /// ["<c>conversion</c>"] Conversion
        /// </summary>
        [Map("conversion")]
        Conversion,
        /// <summary>
        /// ["<c>dividend</c>"] Dividend
        /// </summary>
        [Map("dividend")]
        Dividend,
        /// <summary>
        /// ["<c>reward</c>"] Reward
        /// </summary>
        [Map("reward")]
        Reward,
        /// <summary>
        /// ["<c>creator_fee</c>"] Creator fee
        /// </summary>
        [Map("creator_fee")]
        CreatorFee,
    }

}
