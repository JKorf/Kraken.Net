using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Asset class
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AssetClassExtended>))]
    public enum AssetClassExtended
    {
        /// <summary>
        /// ["<c>tokenized_asset</c>"] Tokenized asset
        /// </summary>
        [Map("tokenized_asset")]
        TokenizedAsset,
        /// <summary>
        /// ["<c>forex</c>"] Forex
        /// </summary>
        [Map("forex")]
        Forex,
        /// <summary>
        /// ["<c>currency</c>"] Currency
        /// </summary>
        [Map("currency")]
        Currency,
        /// <summary>
        /// ["<c>equity</c>"] Equity
        /// </summary>
        [Map("equity")]
        Equity,
        /// <summary>
        /// ["<c>equity_pair</c>"] Equity pair
        /// </summary>
        [Map("equity_pair")]
        EquityPair,
        /// <summary>
        /// ["<c>nft</c>"] Nft
        /// </summary>
        [Map("nft")]
        Nft,
        /// <summary>
        /// ["<c>derivatives</c>"] Derivatives
        /// </summary>
        [Map("derivatives")]
        Derivatives,
        /// <summary>
        /// ["<c>futures_contract </c>"] Futures contract 
        /// </summary>
        [Map("futures_contract ")]
        FuturesContract
    }
}
