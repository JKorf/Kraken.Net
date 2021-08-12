using CryptoExchange.Net.ExchangeInterfaces;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Balance info
    /// </summary>
    public class KrakenBalance: ICommonBalance
    {
        /// <summary>
        /// Asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Balance
        /// </summary>
        public decimal Balance { get; set; }

        string ICommonBalance.CommonAsset => Asset;
        decimal ICommonBalance.CommonAvailable => Balance;
        decimal ICommonBalance.CommonTotal => Balance;
    }
}
