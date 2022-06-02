using System.ComponentModel.DataAnnotations;
using TribeWeather.Common.Interfaces;

namespace TribeWeather.Db.Entities
{
    public class DayWeatherForecast : IDayWeatherForecast
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string LocationId { get; set; } = string.Empty;
        public float TemperatureMin { get; set; }
        public float TemperatureMax { get; set; }
        public float SnowProbability { get; set; }
        public float RainProbability { get; set; }
        public float HoursOfSun { get; set; }
        public float CloudCover { get; set; }
        public string Summary { get; set; } = string.Empty;

        public Location Location { get; set; } = new ();
    }
}