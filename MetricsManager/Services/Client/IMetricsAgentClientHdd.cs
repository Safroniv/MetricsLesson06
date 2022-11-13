using MetricsManager.Models.Requests;

namespace MetricsManager.Services.Client
{
    public interface IMetricsAgentClientHdd

    {
        HddMetricsResponse GetHddMetrics(HddMetricsRequest request);
    }
}
