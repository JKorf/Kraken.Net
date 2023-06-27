using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    public class KrakenFuturesResult
    {
        public IEnumerable<KrakenFuturesError>? Errors { get; set; }
        public string Error { get; set; }
        public bool Success => Result == "success";
        public string Result { get; set; }
        public DateTime ServerTime { get; set; }
    }

    public abstract class KrakenFuturesResult<T> : KrakenFuturesResult
    {
        public abstract T Data { get; set; }
    }

    public class KrakenFuturesError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
