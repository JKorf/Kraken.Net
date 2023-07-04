using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesSymbolResult : KrakenFuturesResult<IEnumerable<KrakenFuturesSymbol>>
    {
        [JsonProperty("instruments")]
        public override IEnumerable<KrakenFuturesSymbol> Data { get; set; } = new List<KrakenFuturesSymbol>();
    }

    /// <summary>
    /// Futures symbol info
    /// </summary>
    public class KrakenFuturesSymbol
    {
        /// <summary>
        /// Category
        /// </summary>
        public string? Category { get; set; }
        /// <summary>
        /// The contract size of the Futures
        /// </summary>
        public decimal? ContractSize { get; set; }
        /// <summary>
        /// Trade precision for the contract (e.g. trade precision of 2 means trades are precise to the hundredth decimal place 0.01).
        /// </summary>
        public decimal? ContractValueTradePrecision { get; set; }
        /// <summary>
        /// Unique identifier of fee schedule associated with the instrument
        /// </summary>
        public string? FeeScheduleUid { get; set; }
        /// <summary>
        /// Funding rate coefficient
        /// </summary>
        public decimal? FundingRateCoefficient { get; set; }
        /// <summary>
        /// Amount of contract used to calculated the mid price for the mark price
        /// </summary>
        public decimal? ImpactMidSize { get; set; }
        /// <summary>
        /// Single-collateral contracts only: Contract's ISIN code
        /// </summary>
        public string? Isin { get; set; }
        /// <summary>
        /// Last trade time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? LastTradingTime { get; set; }
        /// <summary>
        /// A list containing the margin schedules
        /// </summary>
        public IEnumerable<KrakenFutureMarginLevel>? MarginLevels { get; set; }
        /// <summary>
        /// Maximum number of contracts that one can hold in a position
        /// </summary>
        public decimal? MaxPositionSize { get; set; }
        /// <summary>
        /// Perpetuals only: the absolute value of the maximum permissible funding rate
        /// </summary>
        public decimal? MaxRelativeFundingRate { get; set; }
        /// <summary>
        /// When the contract was first available for trading
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? OpeningDate { get; set; }
        /// <summary>
        /// True if the instrument is in post-only mode, false otherwise.
        /// </summary>
        public bool? PostOnly { get; set; }
        /// <summary>
        /// Margin levels for retail clients (investor category no longer eligible for trading).
        /// </summary>
        public IEnumerable<KrakenFutureMarginLevel>? RetailMarginLevels { get; set; }
        /// <summary>
        /// Symbols
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Tag for the contract (currently does not return a value).
        /// </summary>
        public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Tick size of the contract being traded.
        /// </summary>
        public decimal? TickSize { get; set; }
        /// <summary>
        /// True if the instrument can be traded, False otherwise.
        /// </summary>
        public bool Tradeable { get; set; }
        /// <summary>
        /// Type of the instrument
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public InstrumentType Type { get; set; }
        /// <summary>
        /// The underlying of the Futures
        /// </summary>
        public string? Underlying { get; set; }
    }

    /// <summary>
    /// Margin level
    /// </summary>
    public class KrakenFutureMarginLevel
    {
        /// <summary>
        /// The lower limit of the number of contracts to which this margin level applies
        /// </summary>
        public int? Contracts { get; set; }
        /// <summary>
        /// The initial margin requirement for this level
        /// </summary>
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// The maintenance margin requirement for this level
        /// </summary>
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        ///  The lower limit of the number of non-contract units (i.e. quote currency units for linear futures) to which this margin level applies
        /// </summary>
        public decimal NumNonContractUnits { get; set; }
    }
}
