﻿using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info on an asset
    /// </summary>
    public record KrakenAssetInfo
    {
        /// <summary>
        /// Alternative name
        /// </summary>
        [JsonProperty("altname")]
        public string AlternateName { get; set; } = string.Empty;
        /// <summary>
        /// Class of the asset
        /// </summary>
        [JsonProperty("aclass")]
        public string AssetClass { get; set; } = string.Empty;
        /// <summary>
        /// Decimal precision of the asset
        /// </summary>
        public int Decimals { get; set; }
        /// <summary>
        /// Decimals to display
        /// </summary>
        [JsonProperty("display_decimals")]
        public int DisplayDecimals { get; set; }
        /// <summary>
        /// Collateral value
        /// </summary>
        [JsonProperty("collateral_value")]
        public decimal? CollateralValue { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public AssetStatus Status { get; set; }
    }
}
