using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// This defines how the trailing trigger price is calculated from the requested trigger signal. For example, if the max deviation is set to 10, the unit is 'PERCENT', and the underlying order is a sell, then the trigger price will never be more then 10% below the trigger signal. Similarly, if the deviation is 100, the unit is 'QUOTE_CURRENCY', the underlying order is a sell, and the contract is quoted in USD, then the trigger price will never be more than $100 below the trigger signal.
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TrailingStopDeviationUnit>))]
    public enum TrailingStopDeviationUnit
    {
        /// <summary>
        /// Percentage
        /// </summary>
        [Map("PERCENT")]
        Percent,
        /// <summary>
        /// Quote currency
        /// </summary>
        [Map("QUOTE_CURRENCY")]
        QuoteCurrency
    }
}
