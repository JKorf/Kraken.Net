using Kraken.Net.Objects.Models.Futures;
using System;

namespace Kraken.Net.Converters
{
    internal class KrakenFuturesBalancesConverter : JsonConverter<KrakenBalances>
    {
        //public override bool CanConvert(Type objectType)
        //{
        //    return typeof(KrakenBalances).IsAssignableFrom(objectType);
        //}

        //public override object ReadJson(JsonReader reader,
        //    Type objectType, object? existingValue, JsonSerializer serializer)
        //{
        
        //}

        //public override bool CanWrite
        //{
        //    get { return false; }
        //}

        //public override void WriteJson(JsonWriter writer,
        //    object? value, JsonSerializer serializer)
        //{
        //    throw new NotImplementedException();
        //} 

        public override KrakenBalances? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var doc = JsonDocument.ParseValue(ref reader);
            KrakenBalances? balances = null;
            var type = doc.RootElement.GetProperty("type").GetString();

            switch (type)
            {
                case "marginAccount":
                    balances = doc.Deserialize<KrakenMarginAccountBalances>();
                    break;
                case "cashAccount":
                    balances = doc.Deserialize<KrakenCashBalances>();
                    break;
                case "multiCollateralMarginAccount":
                    balances = doc.Deserialize<KrakenMultiCollateralMarginBalances>();
                    break;
            }
            return balances;
        }

        public override void Write(Utf8JsonWriter writer, KrakenBalances value, JsonSerializerOptions options)
        {
            writer.WriteNullValue();
        }
    }
}
