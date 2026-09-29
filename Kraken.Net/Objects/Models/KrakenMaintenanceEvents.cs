using Kraken.Net.Objects.Models.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Models
{
    internal record KrakenMaintenanceEvents
    {
        [JsonPropertyName("events")]
        public KrakenPlannedMaintenance[] Events { get; set; } = [];
    }
}
