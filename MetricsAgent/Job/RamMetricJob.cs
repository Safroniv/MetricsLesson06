using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Job
{
    public class RamMetricJob : IJob
    {
        private PerformanceCounter _ramCounter;
        private IServiceScopeFactory _serviceScopeFactory;

        public RamMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            /*
             
                         _cpuCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
            _cpuCounter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", "_Global_");
             
             */
        }


        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var ramMetricsRepository = serviceScope.ServiceProvider.GetService<IRamMetricsRepository>();
                try
                {
                    var ramUsageInPercents = _ramCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {ramUsageInPercents}");
                    ramMetricsRepository.Create(new Models.RamMetric
                    {
                        Value = (int)ramUsageInPercents,
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
