using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TribeWeather.Common.Interfaces;
using TribeWeather.Db;
using TribeWeather.Db.Entities;

namespace TribeWeather.HostedServices
{
    public class WeatherCacheUpdater : IHostedService, IDisposable
    {
        private int _executionCount;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherCacheUpdater> _logger;
        private readonly IMapper _mapper;
        private Timer? _timer;
        private readonly int _refreshSpanSec;

        public WeatherCacheUpdater(
            IServiceProvider serviceProvider,
            IWeatherService weatherService,
            ILogger<WeatherCacheUpdater> logger,
            IConfiguration configuration, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _weatherService = weatherService;
            _logger = logger;
            _mapper = mapper;
            _refreshSpanSec = ((IConfigurationRoot)configuration).GetValue<int>("DataRefreshIntervalSeconds");
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service WeatherCacheUpdater running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_refreshSpanSec));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            int count = Interlocked.Increment(ref _executionCount);

            _logger.LogInformation("Timed Hosted Service WeatherCacheUpdater is working. Count: {Count}", count);

            using IServiceScope scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<TribeWeatherDbContext>();
            
            List<Location> locations = context!.Locations.ToList();
            
            foreach (Location location in locations)
            {
                IEnumerable<IDayWeatherForecast> newData = _weatherService.GetWeatherForecastAsync(location.LocationId, 5).Result.ToList();
                if (!newData.Any()) continue;

                location.DayForecasts = newData.Select(x => _mapper.Map<DayWeatherForecast>(x)).ToList();
                location.LastUpdated = DateTimeOffset.Now;
            }

            context.SaveChanges();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service WeatherCacheUpdater is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}