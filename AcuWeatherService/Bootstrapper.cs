using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcuWeatherService
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddAcuWeatherConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AcuWeatherSettings>(configuration.GetSection(AcuWeatherSettings.AcuWeather));
            return services;
        }
    }
}
