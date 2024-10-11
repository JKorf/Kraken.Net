using Kraken.Net.Objects.Models;

namespace Kraken.Net.Converters
{
    internal class ExtendedDictionaryConverter<T, U>: JsonConverter<T> where T : KrakenDictionaryResult<U>
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var doc = JsonDocument.ParseValue(ref reader);
            var inner = doc.RootElement.EnumerateObject().First().Value;

            var data = inner.Deserialize<U>();
            var result = (T)Activator.CreateInstance(typeToConvert);
            result.Data = data!;
            if (doc.RootElement.TryGetProperty("last", out var lastElement))
            {
                DateTime last = default;
                if (lastElement.ValueKind == JsonValueKind.Number)
                {
                    var intVal = lastElement.GetInt32();
                    last = DateTimeConverter.ConvertFromSeconds(intVal);
                }
                else
                {
                    var strVal = lastElement.GetString();
                    last = DateTimeConverter.ParseFromString(strVal!);
                }
                result.LastUpdateTime = last;
            }

            return (T)Convert.ChangeType(result, typeToConvert);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("data");
            writer.WriteRawValue(JsonSerializer.Serialize(value.Data));
            writer.WriteNumber("last", (long)DateTimeConverter.ConvertToSeconds(value.LastUpdateTime));
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Dictionary result
    /// </summary>
    /// <typeparam name="T">Type of the data</typeparam>
    public class KrakenDictionaryResult<T>
    {
        /// <summary>
        /// The data
        /// </summary>
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
        /// <summary>
        /// The timestamp of the data
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("last")]
        public DateTime LastUpdateTime { get; set; }
    }

    /// <summary>
    /// Kline result
    /// </summary>
    [JsonConverter(typeof(ExtendedDictionaryConverter<KrakenKlinesResult, IEnumerable<KrakenKline>>))]
    public class KrakenKlinesResult : KrakenDictionaryResult<IEnumerable<KrakenKline>>
    {
    }

    /// <summary>
    /// Trade result
    /// </summary>
    [JsonConverter(typeof(ExtendedDictionaryConverter<KrakenTradesResult, IEnumerable<KrakenTrade>>))]
    public class KrakenTradesResult : KrakenDictionaryResult<IEnumerable<KrakenTrade>>
    {
    }

    /// <summary>
    /// Spread result
    /// </summary>
    [JsonConverter(typeof(ExtendedDictionaryConverter<KrakenSpreadsResult, IEnumerable<KrakenSpread>>))]
    public class KrakenSpreadsResult : KrakenDictionaryResult<IEnumerable<KrakenSpread>>
    {
    }
}
