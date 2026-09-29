using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Account wallet type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AccountWalletType>))]
    public enum AccountWalletType
    {
        /// <summary>
        /// ["<c>main</c>"] Main
        /// </summary>
        [Map("main")]
        Main,
        /// <summary>
        /// ["<c>spot</c>"] Spot
        /// </summary>
        [Map("spot")]
        Spot,
        /// <summary>
        /// ["<c>pay</c>"] Pay
        /// </summary>
        [Map("pay")]
        Pay,
        /// <summary>
        /// ["<c>prop_paper</c>"] Prop paper account
        /// </summary>
        [Map("prop_paper")]
        PropPaper,
        /// <summary>
        /// ["<c>prop_real</c>"] Prop real account
        /// </summary>
        [Map("prop_real")]
        PropReal,
        /// <summary>
        /// ["<c>unknown</c>"] Unknown
        /// </summary>
        [Map("unknown")]
        Unknown,
    }
}
