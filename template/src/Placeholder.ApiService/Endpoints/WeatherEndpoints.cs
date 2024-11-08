using Microsoft.AspNetCore.Mvc;
using Placeholder.Core;
using Placeholder.Core.Models;

namespace Placeholder.ApiService.Endpoints;
public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/weather", async ([FromServices] IWeatherService weatherService) =>
        {
            return await weatherService.GetWeather();
        });

    }
}