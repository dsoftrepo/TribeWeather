using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TribeWeather.Common.Interfaces;

namespace AcuWeatherService
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WeatherService> _logger;
        private readonly IOptions<AcuWeatherSettings> _settings;

        public WeatherService(IOptions<AcuWeatherSettings> settings, IHttpClientFactory httpClientFactory, ILogger<WeatherService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _settings = settings;
        }

        public async Task<IDictionary<string, string>> GetLocationsAsync(string? phrase = null)
        {
            var locationsDictionary = new Dictionary<string, string>();

            try
            {
                string url = phrase == null
                    ? $"{_settings.Value.TopLocations}?apikey={_settings.Value.ApiKey}"
                    : $"{_settings.Value.Locations}?apikey={_settings.Value.ApiKey}&q={phrase}";

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    string msg = await httpResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogWarning(msg);
                    return locationsDictionary;
                }
            
                await using Stream contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                JsonArray locations = JsonNode.Parse(contentStream)!.AsArray();
                foreach (JsonNode? location in locations)
                {
                    if(location?["Key"] == null || location["LocalizedName"] == null) continue;

                    var cityNameInfo = $"{location["LocalizedName"]!.GetValue<string>()} - {location["Country"]!["LocalizedName"]!.GetValue<string>()}"; 

                    locationsDictionary.Add(location["Key"]!.GetValue<string>(), cityNameInfo);
                }

                return locationsDictionary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return locationsDictionary;
        }

        public async Task<IEnumerable<IDayWeatherForecast>> GetWeatherForecastAsync(string location, int days = 1)
        {
            var dailyForecasts = new List<DayWeatherForecast>();

            try
            {
                string url = days > 1 ? _settings.Value.Forecast5d : _settings.Value.Forecast1d;
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{url}{location}?details=true&metric=true&apikey={_settings.Value.ApiKey}");
                HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    string msg = await httpResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogWarning(msg);
                    return dailyForecasts;
                }
                
                await using Stream contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                
                JsonNode forecastNode = JsonNode.Parse(contentStream)!;
                JsonArray? dayForecasts = forecastNode["DailyForecasts"]?.AsArray();

                if (dayForecasts == null)
                    return dailyForecasts;
                    
                foreach (JsonNode? dayForecast in dayForecasts)
                {
                    if (dayForecast == null) continue;
                            
                    dailyForecasts.Add(new DayWeatherForecast
                    {
                        Date = dayForecast["Date"]!.GetValue<DateTimeOffset>(),
                        LocationId = location,
                        TemperatureMin = dayForecast["Temperature"]!["Minimum"]!["Value"]!.GetValue<float>(),
                        TemperatureMax = dayForecast["Temperature"]!["Maximum"]!["Value"]!.GetValue<float>(),
                        RainProbability = dayForecast["Day"]!["RainProbability"]!.GetValue<float>(),
                        SnowProbability = dayForecast["Day"]!["SnowProbability"]!.GetValue<float>(),
                        HoursOfSun = dayForecast["HoursOfSun"]!.GetValue<float>(),
                        CloudCover = dayForecast["Day"]!["CloudCover"]!.GetValue<float>(),
                        Summary = dayForecast["Day"]!["ShortPhrase"]!.GetValue<string>()
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return dailyForecasts;
        }     

        private class DayWeatherForecast : IDayWeatherForecast
        {
            public string LocationId { get; init; }  = string.Empty;
            public DateTimeOffset Date { get; init; }
            public float TemperatureMin { get; init; }
            public float TemperatureMax { get; init; }
            public float SnowProbability { get; init; }
            public float RainProbability { get; init; }
            public float HoursOfSun { get; init; }
            public float CloudCover { get; init; }
            public string Summary { get; init; }  = string.Empty;
        }
    }
}