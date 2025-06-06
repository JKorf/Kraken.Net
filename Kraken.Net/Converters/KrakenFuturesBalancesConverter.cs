﻿using Kraken.Net.Objects.Models.Futures;
using System.Text.Json.Serialization.Metadata;

namespace Kraken.Net.Converters
{
    internal class KrakenFuturesBalancesConverter : JsonConverter<KrakenFuturesBalances>
    {
        public override KrakenFuturesBalances? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var doc = JsonDocument.ParseValue(ref reader);
            var result = new KrakenFuturesBalances();
            var marginAccounts = new List<KrakenMarginAccountBalances>();
            foreach (var element in doc.RootElement.EnumerateObject())
            {
                var type = element.Value.GetProperty("type").GetString();

                switch (type)
                {
                    case "marginAccount":
                        var balance = element.Value.Deserialize<KrakenMarginAccountBalances>((JsonTypeInfo<KrakenMarginAccountBalances>)options.GetTypeInfo(typeof(KrakenMarginAccountBalances)))!;
                        balance.Symbol = element.Name;
                        marginAccounts.Add(balance);
                        break;
                    case "cashAccount":
                        result.CashAccount = element.Value.Deserialize<KrakenCashBalances>((JsonTypeInfo<KrakenCashBalances>)options.GetTypeInfo(typeof(KrakenCashBalances)))!;
                        break;
                    case "multiCollateralMarginAccount":
                        result.MultiCollateralMarginAccount = element.Value.Deserialize<KrakenMultiCollateralMarginBalances>((JsonTypeInfo<KrakenMultiCollateralMarginBalances>)options.GetTypeInfo(typeof(KrakenMultiCollateralMarginBalances)))!;
                        break;
                }
            }

            result.MarginAccounts = marginAccounts.ToArray();
            return result;
        }

        public override void Write(Utf8JsonWriter writer, KrakenFuturesBalances value, JsonSerializerOptions options)
        {
            writer.WriteNullValue();
        }
    }
}
