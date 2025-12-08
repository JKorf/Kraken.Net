using Kraken.Net.Objects.Models;
using System.Text.Json.Serialization.Metadata;

namespace Kraken.Net.Converters
{
    internal class KrakenOrderDescriptionConverter : JsonConverter<KrakenOrderInfo>
    {
        public override KrakenOrderInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDoc = JsonDocument.ParseValue(ref reader);
            var orderInfo = jsonDoc.Deserialize((JsonTypeInfo<KrakenOrderInfo>)options.GetTypeInfo(typeof(KrakenOrderInfo)));
            if (orderInfo == null)
                return null;

            var priceValue = jsonDoc.RootElement.GetProperty("price").GetString();
            orderInfo.TrailingStopDeviationUnit = priceValue?.EndsWith("%") == true ? Enums.TrailingStopDeviationUnit.Percent : Enums.TrailingStopDeviationUnit.QuoteCurrency;
            orderInfo.TrailingStopSign = priceValue?.StartsWith("-") == true ? Enums.TrailingStopSign.Minus : Enums.TrailingStopSign.Plus;
            orderInfo.Price = ExchangeHelpers.ParseDecimal(priceValue?.TrimEnd('%')) ?? 0;
            return orderInfo;
        }

        public override void Write(Utf8JsonWriter writer, KrakenOrderInfo value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, (JsonTypeInfo<KrakenOrderInfo>)options.GetTypeInfo(typeof(KrakenOrderInfo)));
        }
    }
}
