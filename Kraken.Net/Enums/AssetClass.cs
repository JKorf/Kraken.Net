using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Asset class
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AssetClass>))]
    public enum AssetClass
    {
        /// <summary>
        /// Tokenized asset
        /// </summary>
        [Map("tokenized_asset")]
        TokenizedAsset,
        /// <summary>
        /// Forex
        /// </summary>
        [Map("forex")]
        Forex
    }
}
