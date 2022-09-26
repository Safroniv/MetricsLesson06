namespace MetricsAgent.Models.Requests
{
    public class DotNetMetricCreateRequest
    {
        public int Value { get; set; }

        public TimeSpan Time { get; set; }
    }
}
