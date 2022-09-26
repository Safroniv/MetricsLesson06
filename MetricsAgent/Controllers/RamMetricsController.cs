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
    public class RamMetricsController : ControllerBase
    {
        #region Services
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IRamMetricsRepository _ramMetricsRepository;
        private readonly IMapper _mapper;
        #endregion

        public RamMetricsController(
            IRamMetricsRepository ramMetricsRepository,
            ILogger<RamMetricsController> logger,
            IMapper mapper)
        {
            _ramMetricsRepository = ramMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            _ramMetricsRepository.Create(_mapper.Map<RamMetric>(request));
            return Ok();
        }

        /// <summary>
        /// Получить статистику по нагрузке на сеть за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<RamMetricDto>> GetRamMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get ram metrics call.");
            return Ok(_ramMetricsRepository.GetByTimePeriod(fromTime, toTime)
                .Select(metric => _mapper.Map<RamMetricDto>(metric)).ToList());
        }

        [HttpGet("all")]
        public ActionResult<IList<RamMetricDto>> GetAllRamMetrics()
        {
            return Ok(_ramMetricsRepository.GetAll()
                .Select(metric => _mapper.Map<RamMetricDto>(metric)).ToList());
        }
    }
}
