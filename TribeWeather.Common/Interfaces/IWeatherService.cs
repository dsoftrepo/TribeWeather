
namespace TribeWeather.Common.Interfaces
{
    public interface IWeatherService
    {
        Task<IEnumerable<IDayWeatherForecast>> GetWeatherForecastAsync(string location, int days);
        Task<IDictionary<string, string>> GetLocationsAsync(string? phrase = null);
    }
}
