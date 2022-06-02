using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TribeWeather.Common.Dto;
using TribeWeather.Common.Interfaces;
using TribeWeather.Db;
using TribeWeather.Db.Entities;

namespace TribeWeather.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly TribeWeatherDbContext _dbContext;
        private readonly IWeatherService _weatherService;
        private readonly IMapper _mapper;

        public WeatherForecastController(
            TribeWeatherDbContext dbContext,
            IWeatherService weatherService,
            IMapper mapper
            )
        {
            _dbContext = dbContext;
            _weatherService = weatherService;
            _mapper = mapper;
        }

        [HttpGet("toplocations")]
        public async Task<IDictionary<string, string>> TopLocations()
        { 
            return await _weatherService.GetLocationsAsync();
        }

        [HttpGet("locations/{phrase}")]
        public async Task<IDictionary<string, string>> SearchLocations(string? phrase)
        { 
            return await _weatherService.GetLocationsAsync(phrase);
        }

        [HttpGet("{locationId}/{days?}")]
        public async Task<IEnumerable<IDayWeatherForecast>> GetForecast(string locationId, int days=1)
        {
            Location? location = await _dbContext
                .Locations
                .Include(x => x.DayForecasts)
                .Where(x => x.LocationId == locationId)
                .FirstOrDefaultAsync();

            if (location == null)
            {
                IEnumerable<IDayWeatherForecast> newData = await _weatherService.GetWeatherForecastAsync(locationId, days);

                location = new Location(locationId)
                {
                    DayForecasts = newData.Select(x => _mapper.Map<DayWeatherForecast>(x)).ToList(),
                    LastUpdated = DateTimeOffset.Now
                };
            
                _dbContext.Locations.Add(location);
                await _dbContext.SaveChangesAsync();
            }
            // after adding WeatherCacheUpdater timed hosted service this will never occur
            else if (location.LastUpdated < DateTimeOffset.Now.AddHours(-4) || location.DayForecasts.Count < days)
            {
                IEnumerable<IDayWeatherForecast> newData = await _weatherService.GetWeatherForecastAsync(locationId, days);
                
                location.DayForecasts = newData.Select(x => _mapper.Map<DayWeatherForecast>(x)).ToList();
                await _dbContext.SaveChangesAsync();
            }

            return location.DayForecasts
                .Take(days)
                .Select(x=> _mapper.Map<DayWeatherForecastDto>(x));
        }
    }
}