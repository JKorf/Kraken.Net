using System;
using System.Collections.Generic;
using System.Text;
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
        public string Asset { get; set; }
        /// <summary>
        /// Balance
        /// </summary>
        public decimal Balance { get; set; }

        string ICommonBalance.CommonAsset => Asset;
        decimal ICommonBalance.CommonAvailable => Balance;
        decimal ICommonBalance.CommonTotal => Balance;
    }
}
