using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Data page with a cusor for pagination
    /// </summary>
    public class KrakenCursorPage<T>
    {
        /// <summary>
        /// Cursor for the next page
        /// </summary>
        [JsonProperty("next_cursor")]
        public string? NextCursor { get; set; }

        /// <summary>
        /// Page data
        /// </summary>
        [JsonProperty("items")]
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
    }
}
