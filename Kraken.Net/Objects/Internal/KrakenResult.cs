using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenResult
    {
        public IEnumerable<string> Error { get; set; } = Array.Empty<string>();
    }

    internal class KrakenResult<T>: KrakenResult
    {
        public T Result { get; set; } = default!;
    }
}
