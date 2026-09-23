using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Futures position update reason
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KrakenFuturesPositionUpdateReason>))]
    public enum KrakenFuturesPositionUpdateReason
    {
        /// <summary>
        /// Trade
        /// </summary>
        [Map("trade")]
        Trade,
        /// <summary>
        /// Funding realization
        /// </summary>
        [Map("fundingRealisation")]
        FundingRealization,
        /// <summary>
        /// Settlement
        /// </summary>
        [Map("settlement")]
        Settlement,
        /// <summary>
        /// Unknown reason
        /// </summary>
        [Map("unknown")]
        Unknown
    }
}
