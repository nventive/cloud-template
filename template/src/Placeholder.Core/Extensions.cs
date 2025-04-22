using System.Data.Common;
using Placeholder.Core;
using Placeholder.Core.BlobDemo;
using Npgsql;

namespace Microsoft.Extensions.Hosting;

public static class Extensions
{
    public static IHostApplicationBuilder AddCoreServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDataSource("weather");
        builder.Services.AddScoped<DbConnection>(serviceProvider =>
        {
            var dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();
            return dataSource.CreateConnection();
        });

        builder.AddAzureBlobClient("images");

        builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
        builder.Services.AddScoped<IWeatherService, WeatherService>();
        builder.Services.AddScoped<IBlobDemoService, BlobDemoService>();
        return builder;
    }
}
