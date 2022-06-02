using TribeWeather.Common.Interfaces;

namespace TribeWeather.Common.Dto
{
    public class DayWeatherForecastDto : IDayWeatherForecast
    {
        public DateTimeOffset Date { get; set; }
        public string LocationId { get; set; } = string.Empty;
        public float TemperatureMin { get; set; }
        public float TemperatureMax { get; set; }
        public float SnowProbability { get; set; }
        public float RainProbability { get; set; }
        public float HoursOfSun { get; set; }
        public float CloudCover { get; set; }
        public string Summary { get; set; } = string.Empty;

        public bool IsSunny => HoursOfSun > 0 && CloudCover < 50;
        public bool IsSnowy => SnowProbability > 50;
        public bool IsRainy => RainProbability > 50;
    }
}