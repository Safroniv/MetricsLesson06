using AutoMapper;
using MetricsAgent.Converters;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DotnetMetricsController : ControllerBase
    {
        #region Services
        private readonly ILogger<DotnetMetricsController> _logger;
        private readonly IDotnetMetricsRepository _dotnetMetricsRepository;
        private readonly IMapper _mapper;
        #endregion

        public DotnetMetricsController(
            IDotnetMetricsRepository dotnetMetricsRepository,
            ILogger<DotnetMetricsController> logger,
            IMapper mapper)
        {
            _dotnetMetricsRepository = dotnetMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            _dotnetMetricsRepository.Create(_mapper.Map<DotnetMetric>(request));
            return Ok();
        }

        /// <summary>
        /// Получить статистику по нагрузке на сеть за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<DotnetMetricDto>> GetNetworkMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get network metrics call.");
            return Ok(_dotnetMetricsRepository.GetByTimePeriod(fromTime, toTime)
                .Select(metric => _mapper.Map<DotnetMetricDto>(metric)).ToList());
        }

        [HttpGet("all")]
        public ActionResult<IList<DotnetMetricDto>> GetAllnDotetMetrics()
        {
            return Ok(_dotnetMetricsRepository.GetAll()
                .Select(metric => _mapper.Map<DotnetMetricDto>(metric)).ToList());
        }
    }
}
