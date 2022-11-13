using MetricsManager.Models;
using MetricsManager.Models.Requests;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace MetricsManager.Services.Client.Impl
{
    public class MetricsAgentClientHdd : IMetricsAgentClientHdd
    {

        #region Services

        private readonly AgentPool _agentPool;
        private readonly HttpClient _httpClient;

        #endregion

        public MetricsAgentClientHdd(HttpClient httpClient,
            AgentPool agentPool)
        {
            _httpClient = httpClient;
            _agentPool = agentPool;
        }

        public HddMetricsResponse GetHddMetrics(HddMetricsRequest request)
        {
            AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == request.AgentId);
            if (agentInfo == null)
                return null;

            string requestStr =
                $"{agentInfo.AgentAddress}api/metrics/hdd/from/{request.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{request.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestStr);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = _httpClient.Send(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;
                HddMetricsResponse hddMetricsResponse =
                    (HddMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(HddMetricsResponse));
                hddMetricsResponse.AgentId = request.AgentId;
                return hddMetricsResponse;
            }

            return null;
        }
    }
}
