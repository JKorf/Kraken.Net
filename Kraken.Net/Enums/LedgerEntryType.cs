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
        /// Trade change
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
        /// Transfer
        /// </summary>
        [Map("transfer")]
        Transfer,
        /// <summary>
        /// Rollover
        /// </summary>
        [Map("rollover")]
        Rollover,
        /// <summary>
        /// Spend
        /// </summary>
        [Map("spend")]
        Spend,
        /// <summary>
        /// Receive
        /// </summary>
        [Map("receive")]
        Receive,
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
        /// None
        /// </summary>
        [Map("none")]
        None,
        /// <summary>
        /// Credit
        /// </summary>
        [Map("credit")]
        Credit,
        /// <summary>
        /// Dividend
        /// </summary>
        [Map("dividend")]
        Dividend,
        /// <summary>
        /// Sale
        /// </summary>
        [Map("sale")]
        Sale,
        /// <summary>
        /// Reward
        /// </summary>
        [Map("reward")]
        Reward,
        /// <summary>
        /// Conversion
        /// </summary>
        [Map("conversion")]
        Conversion,
        /// <summary>
        /// NFT Trade
        /// </summary>
        [Map("nfttrade")]
        NftTrade,
        /// <summary>
        /// NFT Creator fee
        /// </summary>
        [Map("nftcreatorfee")]
        NftCreatorFee,
        /// <summary>
        /// NFT rebate
        /// </summary>
        [Map("nftrebate")]
        NftRebate,
        /// <summary>
        /// Custody transfer
        /// </summary>
        [Map("custodytransfer")]
        CustodyTransfer
    }
}
