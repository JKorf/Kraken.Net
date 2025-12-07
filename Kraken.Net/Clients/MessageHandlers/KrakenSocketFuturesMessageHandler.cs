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

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [


             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("event"),
                    new PropertyFieldReference("feed"),
                    new PropertyFieldReference("product_ids") { ArrayValues = true } ,
                ],
                TypeIdentifierCallback = x => {
                    var result = $"{x.FieldValue("event")}-{x.FieldValue("feed")}";
                    var productIds = x.FieldValue("product_ids")?.Split(',');
                    if (productIds?.Length > 0)
                        result += "-" + string.Join("-", productIds.Select(i => i!.ToLowerInvariant()));
                    return result;
                }
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("event"),
                    new PropertyFieldReference("feed"),
                ],
                TypeIdentifierCallback = x => $"{x.FieldValue("event")}-{x.FieldValue("feed")}"
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("feed"),
                    new PropertyFieldReference("product_id"),
                ],
                TypeIdentifierCallback = x => $"{x.FieldValue("feed")}-{x.FieldValue("product_id")!.ToLowerInvariant()}"
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("feed"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("feed")!
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("event"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("event")!
            },


        ];
    }
}
