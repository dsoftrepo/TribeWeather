namespace TribeWeather.Common.Interfaces;

public interface IDayWeatherForecast
{
    string LocationId { get; }

    DateTimeOffset Date { get; }

    float TemperatureMin { get; }
    float TemperatureMax { get; }
    
    float SnowProbability { get; }
    float RainProbability { get; }
    
    float HoursOfSun { get; }
    float CloudCover { get; }
    
    string Summary { get; }
}