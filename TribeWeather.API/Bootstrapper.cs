using AcuWeatherService;
using Microsoft.Extensions.DependencyInjection;
using TribeWeather.Common.Interfaces;

namespace TribeWeather.API
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IWeatherService, WeatherService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
