using System;
using CryptoExchange.Net.Converters;
using Kraken.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kraken.Net.Converters
{
    internal class ExtendedDictionaryConverter<T>: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var data = (KrakenDictionaryResult<T>) value;
            writer.WriteStartObject();
            writer.WritePropertyName("data");
            writer.WriteRawValue(JsonConvert.SerializeObject(data.Data));
            writer.WritePropertyName("last");
            writer.WriteValue(JsonConvert.SerializeObject(data.Last, new TimestampSecondsConverter()));
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var inner = obj.First;
            var data = inner.First.ToObject<T>();
            var result = (KrakenDictionaryResult<T>)Activator.CreateInstance(objectType);
            result.Data = data;
            var timestamp = (long)obj["last"];
            if(timestamp > 1000000000000000000)
                result.Last = obj["last"].ToObject<DateTime>(new JsonSerializer() {Converters = {new TimestampNanoSecondsConverter()}});
            else
                result.Last = obj["last"].ToObject<DateTime>(new JsonSerializer() {Converters = {new TimestampSecondsConverter()}});
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
        public T Data { get; set; }
        /// <summary>
        /// The timestamp of the data
        /// </summary>
        [JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Last { get; set; }
    }

    /// <summary>
    /// Kline result
    /// </summary>
    [JsonConverter(typeof(ExtendedDictionaryConverter<KrakenKline[]>))]
    public class KrakenKlinesResult : KrakenDictionaryResult<KrakenKline[]>
    {
    }

    /// <summary>
    /// Trade result
    /// </summary>
    [JsonConverter(typeof(ExtendedDictionaryConverter<KrakenTrade[]>))]
    public class KrakenTradesResult : KrakenDictionaryResult<KrakenTrade[]>
    {
    }

    /// <summary>
    /// Spread result
    /// </summary>
    [JsonConverter(typeof(ExtendedDictionaryConverter<KrakenSpread[]>))]
    public class KrakenSpreadsResult : KrakenDictionaryResult<KrakenSpread[]>
    {
    }
}
