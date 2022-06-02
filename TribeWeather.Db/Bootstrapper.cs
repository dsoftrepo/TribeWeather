using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TribeWeather.Db
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddTribeDBContext(this IServiceCollection services, string dbName)
        {
            services.AddDbContext<TribeWeatherDbContext>(opt => opt.UseInMemoryDatabase(dbName));
            return services;
        }
    }
}
