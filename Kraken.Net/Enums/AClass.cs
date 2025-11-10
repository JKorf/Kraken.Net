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
    [JsonConverter(typeof(EnumConverter<AClass>))]
    public enum AClass
    {
        /// <summary>
        /// Tokenized asset (xstocks)
        /// </summary>
        [Map("tokenized_asset")]
        TokenizedAsset,
        /// <summary>
        /// Spot currency pairs
        /// </summary>
        [Map("currency")]
        Currency
    }
}
