﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Placed order info
    /// </summary>
    public record KrakenPlacedOrder
    {
        /// <summary>
        /// Order ids
        /// </summary>
        [JsonProperty("txid")]
        public IEnumerable<string> OrderIds { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Descriptions
        /// </summary>
        [JsonProperty("descr")]
        public KrakenPlacedOrderDescription Descriptions { get; set; } = default!;
    }

    /// <summary>
    /// Order descriptions
    /// </summary>
    public record KrakenPlacedOrderDescription
    {
        /// <summary>
        /// Order description
        /// </summary>
        [JsonProperty("order")]
        public string OrderDescription { get; set; } = string.Empty;
        /// <summary>
        /// Close order description
        /// </summary>
        [JsonProperty("close")]
        public string CloseOrderDescription { get; set; } = string.Empty;
    }
}
