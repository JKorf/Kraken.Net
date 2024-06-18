using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesSelfTradeResult : KrakenFuturesResult<SelfTradeStrategy>
    {
        [JsonProperty("strategy")]
        [JsonConverter(typeof(EnumConverter))]
        public override SelfTradeStrategy Data { get; set; }
    }
}
