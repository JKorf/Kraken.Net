using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Trigger options
    /// </summary>
    public record KrakenTriggerOptions
    {
        /// <summary>
        /// Trigger price
        /// </summary>
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// Trigger side
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public TriggerSide TriggerSide { get; set; }
        /// <summary>
        /// Trigger signal
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public TriggerSignal TriggerSignal { get; set; }
    }
}
