using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesResult
    {
        public IEnumerable<KrakenFuturesError>? Errors { get; set; }
        public string? Error { get; set; }
        public bool Success => string.Equals(Result, "success", StringComparison.Ordinal);
        public string? Result { get; set; }
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ServerTime { get; set; }
    }

    internal abstract record KrakenFuturesResult<T> : KrakenFuturesResult
    {
        public abstract T Data { get; set; }
    }

    internal record KrakenFuturesError
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
