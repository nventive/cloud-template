using Microsoft.AspNetCore.Mvc;
using Placeholder.Core;
using Placeholder.Core.Models;

namespace Placeholder.ApiService.Endpoints;
public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/weathers", async ([FromServices] IWeatherService weatherService) =>
        {
            Weather[] weathers = await weatherService.GetWeathers();
            return Results.Ok(weathers);
        });

        endpoints.MapGet("/weather/{weatherId}", async (long weatherId, [FromServices] IWeatherService weatherService) =>
        {
            Weather? weather = await weatherService.GetWeather(weatherId);
            return Results.Ok(weather);
        });

        endpoints.MapPost("/weather", async ([FromBody] Weather weather, [FromServices] IWeatherService weatherService) =>
        {
            Weather? weatherCreated = await weatherService.CreateWeather(weather);
            return Results.Ok(weatherCreated);
        });

        endpoints.MapPut("/weather/{weatherId}", async (long weatherId, [FromBody] Weather weather, [FromServices] IWeatherService weatherService) =>
        {
            Weather? weatherUpdated = await weatherService.UpdateWeather(weatherId, weather);
            return Results.Ok(weatherUpdated);
        });

        endpoints.MapDelete("/weather/{weatherId}", async (long weatherId, [FromServices] IWeatherService weatherService) =>
        {
            await weatherService.DeleteWeather(weatherId);
            return Results.Ok();
        });
    }
}