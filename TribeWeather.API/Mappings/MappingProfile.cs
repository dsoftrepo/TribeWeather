using AutoMapper;
using TribeWeather.Common.Dto;
using TribeWeather.Common.Interfaces;
using TribeWeather.Db.Entities;

namespace TribeWeather.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IDayWeatherForecast, DayWeatherForecast>()
                .ReverseMap();
            CreateMap<IDayWeatherForecast, DayWeatherForecastDto>()
                .ReverseMap();
            CreateMap<DayWeatherForecast, DayWeatherForecastDto>()
                .ReverseMap();
        }
    }
}
