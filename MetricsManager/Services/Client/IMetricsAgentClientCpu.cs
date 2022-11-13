using MetricsManager.Models.Requests;

namespace MetricsManager.Services.Client
{
    public interface IMetricsAgentClientCpu
    {
        CpuMetricsResponse GetCpuMetrics(CpuMetricsRequest request);
    }
}
