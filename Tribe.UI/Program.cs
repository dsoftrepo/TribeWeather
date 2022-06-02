using AcuWeatherService;
using TribeWeather.API;
using TribeWeather.Db;
using TribeWeather.HostedServices;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTribeDBContext("TribeWeather");
builder.Services.AddApiServices();
builder.Services.AddControllers();
builder.Services.AddAcuWeatherConfiguration(builder.Configuration);
builder.Services.AddHostedService<WeatherCacheUpdater>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
