namespace AcuWeatherService
{
    public class AcuWeatherSettings
    {
        public const string AcuWeather = "AcuWeather";

        public string ApiKey { get; set; } = string.Empty;
        public string Locations { get; set; } = string.Empty;
        public string TopLocations { get; set; } = string.Empty;
        public string Forecast5d { get; set; } = string.Empty;
        public string Forecast1d { get; set; } = string.Empty;
    }
}
