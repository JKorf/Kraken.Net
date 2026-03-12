using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Category
    /// </summary>
    [JsonConverter(typeof(EnumConverter<BalanceUpdateCategory>))]
    public enum BalanceUpdateCategory
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
        /// ["<c>margin-trade</c>"] Margin trade
        /// </summary>
        [Map("margin-trade")]
        MarginTrade,
        /// <summary>
        /// ["<c>margin-settle</c>"] Margin settle
        /// </summary>
        [Map("margin-settle")]
        MarginSettle,
        /// <summary>
        /// ["<c>margin-conversion</c>"] Margin conversion
        /// </summary>
        [Map("margin-conversion")]
        MarginConversion,
        /// <summary>
        /// ["<c>conversion</c>"] Conversion
        /// </summary>
        [Map("conversion")]
        Conversion,
        /// <summary>
        /// ["<c>credit</c>"] Credit
        /// </summary>
        [Map("credit")]
        Credit,
        /// <summary>
        /// ["<c>marginrollover</c>"] Marginrollover
        /// </summary>
        [Map("marginrollover")]
        Marginrollover,
        /// <summary>
        /// ["<c>staking-rewards</c>"] Staking rewards
        /// </summary>
        [Map("staking-rewards")]
        StakingRewards,
        /// <summary>
        /// ["<c>instant</c>"] Instant
        /// </summary>
        [Map("instant")]
        Instant,
        /// <summary>
        /// ["<c>equity-trade</c>"] Equity trade
        /// </summary>
        [Map("equity-trade")]
        EquityTrade,
        /// <summary>
        /// ["<c>airdrop</c>"] Airdrop
        /// </summary>
        [Map("airdrop")]
        Airdrop,
        /// <summary>
        /// ["<c>equity-dividend</c>"] Equity dividend
        /// </summary>
        [Map("equity-dividend")]
        EquityDividend,
        /// <summary>
        /// ["<c>reward-bonus</c>"] Reward bonus
        /// </summary>
        [Map("reward-bonus")]
        RewardBonus,
        /// <summary>
        /// ["<c>nft</c>"] Nft
        /// </summary>
        [Map("nft")]
        Nft,
        /// <summary>
        /// ["<c>block-trade</c>"] Block trade
        /// </summary>
        [Map("block-trade")]
        BlockTrade,
    }

}
