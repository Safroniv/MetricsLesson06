using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Job
{
    public class DotNetMetricJob : IJob
    {
        private PerformanceCounter _dotnetCounter;
        private IServiceScopeFactory _serviceScopeFactory;

        public DotNetMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _dotnetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
            /*
             
                         _cpuCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
            _cpuCounter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", "_Global_");
             
             */
        }


        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var dotnetMetricsRepository = serviceScope.ServiceProvider.GetService<IDotnetMetricsRepository>();
                try
                {
                    var dotnetUsageInPercents = _dotnetCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {dotnetUsageInPercents}");
                    dotnetMetricsRepository.Create(new Models.DotnetMetric
                    {
                        Value = (int)dotnetUsageInPercents,
                        Time = (long)time.TotalSeconds
                    });
                }
                catch (Exception ex)
                {

                }
            }
               

            return Task.CompletedTask;
        }
    }
}
