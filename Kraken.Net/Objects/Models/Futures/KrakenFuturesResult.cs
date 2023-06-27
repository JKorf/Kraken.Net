using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    public abstract class KrakenFuturesResult<T>
    {
        public IEnumerable<KrakenFuturesError>? Errors { get; set; }
        public bool Success => Result == "success";
        public string Result { get; set; }
        public DateTime ServerTime { get; set; }

        public abstract T Data { get; set; }
    }

    public class KrakenFuturesError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
