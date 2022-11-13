using System.Text.Json.Serialization;

namespace MetricsManager.Models.Requests
{
    public class HddMetricsResponse
    {

        /// <summary>
        /// Идентификатор агента
        /// </summary>
        public int AgentId { get; set; }

        [JsonPropertyName("metrics")]
        public HddMetric[] Metrics { get; set; }
    }
}
