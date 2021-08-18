using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Socket
{
    /// <summary>
    /// Cancel all result
    /// </summary>
    public class KrakenStreamCancelAllResult : KrakenSocketResponseBase
    {
        /// <summary>
        /// Number of orders cancelled
        /// </summary>
        public int Count { get; set; }
    }
}
