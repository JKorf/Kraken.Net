using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesResult
    {
        public IEnumerable<KrakenFuturesError>? Errors { get; set; }
        public string? Error { get; set; }
        public bool Success => Result == "success";
        public string? Result { get; set; }
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ServerTime { get; set; }
    }

    internal abstract class KrakenFuturesResult<T> : KrakenFuturesResult
    {
        public abstract T Data { get; set; }
    }

    internal class KrakenFuturesError
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
