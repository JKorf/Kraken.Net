using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net;
using System;
using System.Linq;
using System.Text.Json;

namespace Kraken.Net.Clients.MessageHandlers
{
    internal class KrakenSocketFuturesMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(KrakenExchange._serializerContext);

        protected override MessageEvaluator[] TypeEvaluators { get; } = [


             new MessageEvaluator {
                Priority = 1,
                Fields = [
                    new PropertyFieldReference("event"),
                    new PropertyFieldReference("feed"),
                    new PropertyFieldReference("product_ids") { ArrayValues = true } ,
                ],
                IdentifyMessageCallback = x => {
                    var result = $"{x.FieldValue("event")}-{x.FieldValue("feed")}";
                    var productIds = x.FieldValue("product_ids")?.Split(',');
                    if (productIds?.Length > 0)
                        result += "-" + string.Join("-", productIds.Select(i => i!.ToLowerInvariant()));
                    return result;
                }
            },

             new MessageEvaluator {
                Priority = 2,
                Fields = [
                    new PropertyFieldReference("event"),
                    new PropertyFieldReference("feed"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("event")}-{x.FieldValue("feed")}"
            },

             new MessageEvaluator {
                Priority = 3,
                Fields = [
                    new PropertyFieldReference("feed"),
                    new PropertyFieldReference("product_id"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("feed")}-{x.FieldValue("product_id")!.ToLowerInvariant()}"
            },

             new MessageEvaluator {
                Priority = 4,
                Fields = [
                    new PropertyFieldReference("feed"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("feed")!
            },

             new MessageEvaluator {
                Priority = 5,
                Fields = [
                    new PropertyFieldReference("event"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("event")!
            },


        ];
    }
}
