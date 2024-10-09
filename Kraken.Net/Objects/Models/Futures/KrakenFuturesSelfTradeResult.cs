using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesSelfTradeResult : KrakenFuturesResult<SelfTradeStrategy>
    {
        [JsonPropertyName("strategy")]
        [JsonConverter(typeof(EnumConverter))]
        public override SelfTradeStrategy Data { get; set; }
    }
}
