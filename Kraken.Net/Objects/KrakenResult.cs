using System.Collections.Generic;

namespace Kraken.Net.Objects
{
    internal class KrakenResult<T>
    {
        public IEnumerable<string> Error { get; set; } = new List<string>();
        public T Result { get; set; } = default!;
    }
}
