using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Placeholder.Core.Weather;

namespace Microsoft.Extensions.Hosting;

public static class Extensions
{
    public static IHostApplicationBuilder AddPlaceholderCoreServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDataSource("weather");
        builder.Services.AddScoped<DbConnection>(serviceProvider => 
        {
            var dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();
            return dataSource.CreateConnection();
        });

        builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>(); 

        return builder;
    }
}