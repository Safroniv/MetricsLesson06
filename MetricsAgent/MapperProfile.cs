using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<CpuMetricCreateRequest, CpuMetric>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (long)src.Time.TotalSeconds));
            //для  network ram hdd 
            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<HddMetricCreateRequest, HddMetric>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (int)src.Time.TotalSeconds));
            CreateMap<DotnetMetric, DotnetMetricDto>();
            CreateMap<DotNetMetricCreateRequest, DotnetMetric>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (int)src.Time.TotalSeconds));
            CreateMap<RamMetric, RamMetricDto>();
            CreateMap<RamMetricCreateRequest, RamMetric>()
                .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(x => x.Time, opt => opt.MapFrom(src => (int)src.Time.TotalSeconds));
        }
    }
}