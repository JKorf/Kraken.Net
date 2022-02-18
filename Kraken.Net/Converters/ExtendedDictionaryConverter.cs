using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Kraken.Net.Objects.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kraken.Net.Converters
{
    internal class ExtendedDictionaryConverter<T>: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var data = (KrakenDictionaryResult<T>) value!;
            writer.WriteStartObject();
            writer.WritePropertyName("data");
            writer.WriteRawValue(JsonConvert.SerializeObject(data.Data));
            writer.WritePropertyName("last");
            writer.WriteValue(DateTimeConverter.ConvertToSeconds(data.LastUpdateTime));
            writer.WriteEndObject();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var inner = obj.First;
            if (inner?.First == null)
                return null;

            var data = inner.First.ToObject<T>();
            var result = (KrakenDictionaryResult<T>)Activator.CreateInstance(objectType);
            result.Data = data!;
            var lastValue = obj["last"];
            if (lastValue != null)
            {
                result.LastUpdateTime = lastValue.ToObject<DateTime>(new JsonSerializer() { Converters = { new DateTimeConverter() } });
            }
            return Convert.ChangeType(result, objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
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
        public T Data { get; set; } = default!;
        /// <summary>
        /// The timestamp of the data
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("last")]
        public DateTime LastUpdateTime { get; set; }
    }

    /// <summary>
    /// Kline result
    /// </summary>
    [JsonConverter(typeof(ExtendedDictionaryConverter<IEnumerable<KrakenKline>>))]
    public class KrakenKlinesResult : KrakenDictionaryResult<IEnumerable<KrakenKline>>
    {
    }

    /// <summary>
    /// Trade result
    /// </summary>
    [JsonConverter(typeof(ExtendedDictionaryConverter<IEnumerable<KrakenTrade>>))]
    public class KrakenTradesResult : KrakenDictionaryResult<IEnumerable<KrakenTrade>>
    {
    }

    /// <summary>
    /// Spread result
    /// </summary>
    [JsonConverter(typeof(ExtendedDictionaryConverter<IEnumerable<KrakenSpread>>))]
    public class KrakenSpreadsResult : KrakenDictionaryResult<IEnumerable<KrakenSpread>>
    {
    }
}
