using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// The type of a ledger entry
    /// </summary>
    [JsonConverter(typeof(EnumConverter<LedgerEntryType>))]
    public enum LedgerEntryType
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
        /// ["<c>trade</c>"] Trade change
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
        /// ["<c>transfer</c>"] Transfer
        /// </summary>
        [Map("transfer")]
        Transfer,
        /// <summary>
        /// ["<c>rollover</c>"] Rollover
        /// </summary>
        [Map("rollover")]
        Rollover,
        /// <summary>
        /// ["<c>spend</c>"] Spend
        /// </summary>
        [Map("spend")]
        Spend,
        /// <summary>
        /// ["<c>receive</c>"] Receive
        /// </summary>
        [Map("receive")]
        Receive,
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
        /// ["<c>none</c>"] None
        /// </summary>
        [Map("none")]
        None,
        /// <summary>
        /// ["<c>credit</c>"] Credit
        /// </summary>
        [Map("credit")]
        Credit,
        /// <summary>
        /// ["<c>dividend</c>"] Dividend
        /// </summary>
        [Map("dividend")]
        Dividend,
        /// <summary>
        /// ["<c>sale</c>"] Sale
        /// </summary>
        [Map("sale")]
        Sale,
        /// <summary>
        /// ["<c>reward</c>"] Reward
        /// </summary>
        [Map("reward")]
        Reward,
        /// <summary>
        /// ["<c>conversion</c>"] Conversion
        /// </summary>
        [Map("conversion")]
        Conversion,
        /// <summary>
        /// ["<c>nfttrade</c>"] NFT Trade
        /// </summary>
        [Map("nfttrade")]
        NftTrade,
        /// <summary>
        /// ["<c>nftcreatorfee</c>"] NFT Creator fee
        /// </summary>
        [Map("nftcreatorfee")]
        NftCreatorFee,
        /// <summary>
        /// ["<c>nftrebate</c>"] NFT rebate
        /// </summary>
        [Map("nftrebate")]
        NftRebate,
        /// <summary>
        /// ["<c>custodytransfer</c>"] Custody transfer
        /// </summary>
        [Map("custodytransfer")]
        CustodyTransfer
    }
}
