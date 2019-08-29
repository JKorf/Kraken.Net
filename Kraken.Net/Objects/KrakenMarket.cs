using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Market info
    /// </summary>
    public class KrakenMarket
    {
        /// <summary>
        /// Alternative name
        /// </summary>
        [JsonProperty("altname")]
        public string AlternateName { get; set; }
        /// <summary>
        /// Name to use for the socket client subscriptions
        /// </summary>
        [JsonProperty("wsname")]
        public string WebsocketName { get; set; }
        /// <summary>
        /// Class of the base asset
        /// </summary>
        [JsonProperty("aclass_base")]
        public string BaseAssetClass { get; set; }
        /// <summary>
        /// Name of the base asset
        /// </summary>
        [JsonProperty("base")]
        public string BaseAsset { get; set; }
        /// <summary>
        /// Class of the quote asset
        /// </summary>
        [JsonProperty("aclass_quote")]
        public string QuoteAssetClass { get; set; }
        /// <summary>
        /// Name of the quote asset
        /// </summary>
        [JsonProperty("quote")]
        public string QuoteAsset { get; set; }
        /// <summary>
        /// Let size
        /// </summary>
        [JsonProperty("lot")]
        public string VolumeLotSize { get; set; }
        /// <summary>
        /// Decimals of the market
        /// </summary>
        [JsonProperty("pair_decimals")]
        public int Decimals { get; set; }
        /// <summary>
        /// Lot decimals
        /// </summary>
        [JsonProperty("lot_decimals")]
        public int LotDecimals { get; set; }
        /// <summary>
        /// Lot multiplier
        /// </summary>
        [JsonProperty("lot_multiplier")]
        public decimal LotMultiplier { get; set; }
        /// <summary>
        /// Buy leverage amounts
        /// </summary>
        [JsonProperty("leverage_buy")]
        public decimal[] LeverageBuy { get; set; }
        /// <summary>
        /// Sell leverage amounts
        /// </summary>
        [JsonProperty("leverage_sell")]
        public decimal[] LeverageSell { get; set; }
        /// <summary>
        /// Fee structure
        /// </summary>
        public List<KrakenFeeEntry> Fees { get; set; }
        /// <summary>
        /// Maker fee structure
        /// </summary>
        [JsonProperty("fees_maker")]
        public List<KrakenFeeEntry> FeesMaker { get; set; }
        /// <summary>
        /// The currency the fee is deducted from
        /// </summary>
        [JsonProperty("fee_volume_currency")]
        public string FeeCurrency { get; set; }
        /// <summary>
        /// Margin call level
        /// </summary>
        [JsonProperty("margin_call")]
        public int MarginCall { get; set; }
        /// <summary>
        /// Stop-out/liquidation margin level
        /// </summary>
        [JsonProperty("margin_stop")]
        public int MarginStop { get; set; }
    }
}
