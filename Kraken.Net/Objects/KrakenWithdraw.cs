using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
	/// <summary>
	/// Order info
	/// </summary>
	public class KrakenWithdraw
	{
		/// <summary>
		/// Reference id
		/// </summary>
		[JsonProperty("refid")]
		public string ReferenceId { get; set; } = "";
	}
}