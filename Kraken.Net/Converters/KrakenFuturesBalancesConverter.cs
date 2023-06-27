using Kraken.Net.Objects.Models.Futures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Kraken.Net.Converters
{
    public class KrakenFuturesBalancesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(KrakenBalances).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            KrakenBalances balances = null!;
            var type = (string?)jo["type"];
            switch (type)
            {
                case "marginAccount":
                    balances = new KrakenMarginAccountBalances();
                    break;
                case "cashAccount":
                    balances = new KrakenCashBalances();
                    break;
                case "multiCollateralMarginAccount":
                    balances = new KrakenMultiCollateralMarginBalances();
                    break;
            }

            serializer.Populate(jo.CreateReader(), balances);
            return balances;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
