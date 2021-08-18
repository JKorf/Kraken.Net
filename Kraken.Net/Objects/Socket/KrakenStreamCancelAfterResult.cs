using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Socket
{
    /// <summary>
    /// Cancel after result
    /// </summary>
    public class KrakenStreamCancelAfterResult : KrakenSocketResponseBase
    {
        /// <summary>
        /// Current time
        /// </summary>
        public DateTime CurrentTime { get; set; }
        /// <summary>
        /// Trigger time
        /// </summary>
        public DateTime TriggerTime { get; set; }
    }
}
