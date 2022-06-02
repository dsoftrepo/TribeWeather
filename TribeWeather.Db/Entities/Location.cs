using System.ComponentModel.DataAnnotations;

namespace TribeWeather.Db.Entities
{
    public class Location
    {
        public Location() { }

        public Location(string locationId)
        {
            LocationId = locationId;
            DayForecasts = new List<DayWeatherForecast>();
        }

        [Key]
        public string LocationId { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public List<DayWeatherForecast> DayForecasts { get; set; }
    }
}
