using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Job
{
    public class HddMetricJob : IJob
    {
        private PerformanceCounter _hddCounter;
        private IServiceScopeFactory _serviceScopeFactory;

        public HddMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hddCounter = new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");
            /*
             
                         _cpuCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
            _cpuCounter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", "_Global_");
             
             */
        }


        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var hddMetricsRepository = serviceScope.ServiceProvider.GetService<IHddMetricsRepository>();
                try
                {
                    var hddUsageInPercents = _hddCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {hddUsageInPercents}");
                    hddMetricsRepository.Create(new Models.HddMetric
                    {
                        Value = (int)hddUsageInPercents,
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
