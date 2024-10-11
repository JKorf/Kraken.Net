using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Category
    /// </summary>
    public enum BalanceUpdateCategory
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
        /// Margin trade
        /// </summary>
        [Map("margin-trade")]
        MarginTrade,
        /// <summary>
        /// Margin settle
        /// </summary>
        [Map("margin-settle")]
        MarginSettle,
        /// <summary>
        /// Margin conversion
        /// </summary>
        [Map("margin-conversion")]
        MarginConversion,
        /// <summary>
        /// Conversion
        /// </summary>
        [Map("conversion")]
        Conversion,
        /// <summary>
        /// Credit
        /// </summary>
        [Map("credit")]
        Credit,
        /// <summary>
        /// Marginrollover
        /// </summary>
        [Map("marginrollover")]
        Marginrollover,
        /// <summary>
        /// Staking rewards
        /// </summary>
        [Map("staking-rewards")]
        StakingRewards,
        /// <summary>
        /// Instant
        /// </summary>
        [Map("instant")]
        Instant,
        /// <summary>
        /// Equity trade
        /// </summary>
        [Map("equity-trade")]
        EquityTrade,
        /// <summary>
        /// Airdrop
        /// </summary>
        [Map("airdrop")]
        Airdrop,
        /// <summary>
        /// Equity dividend
        /// </summary>
        [Map("equity-dividend")]
        EquityDividend,
        /// <summary>
        /// Reward bonus
        /// </summary>
        [Map("reward-bonus")]
        RewardBonus,
        /// <summary>
        /// Nft
        /// </summary>
        [Map("nft")]
        Nft,
        /// <summary>
        /// Block trade
        /// </summary>
        [Map("block-trade")]
        BlockTrade,
    }

}
