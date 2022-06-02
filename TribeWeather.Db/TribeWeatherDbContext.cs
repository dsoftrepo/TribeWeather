using Microsoft.EntityFrameworkCore;
using TribeWeather.Db.Entities;

namespace TribeWeather.Db
{
    public class TribeWeatherDbContext : DbContext
    {
        public TribeWeatherDbContext(DbContextOptions<TribeWeatherDbContext> options) 
            : base(options)  { }

        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<DayWeatherForecast> DayWeatherForecasts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DayWeatherForecast>()
                .HasOne(x=>x.Location)
                .WithMany(x=>x.DayForecasts)
                .HasForeignKey(x=>x.LocationId);
    
            base.OnModelCreating(modelBuilder);
        }
    }
}
