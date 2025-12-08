using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesSelfTradeResult : KrakenFuturesResult<SelfTradeStrategy>
    {
        [JsonPropertyName("strategy")]

        public override SelfTradeStrategy Data { get; set; }
    }
}
