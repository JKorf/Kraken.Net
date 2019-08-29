namespace Kraken.Net.Objects
{
    internal class KrakenResult<T>
    {
        public string[] Error { get; set; }
        public T Result { get; set; }
    }
}
