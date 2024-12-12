using Microsoft.AspNetCore.Mvc;
using Placeholder.Core;
using Placeholder.Core.Models;

namespace Placeholder.ApiService.Endpoints;
public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/weathers", GetAllWeathers);
        endpoints.MapGet("/weather/{weatherId}", GetWeather);
        endpoints.MapPost("/weather", CreateWeather);
        endpoints.MapPut("/weather/{weatherId}", UpdateWeather);
        endpoints.MapDelete("/weather/{weatherId}", DeleteWeather);
    }

    private static async Task<IResult> GetAllWeathers(IWeatherService weatherService)
    {
        try {
            IEnumerable<Weather> weathers = await weatherService.GetWeathers();
            return Results.Ok(weathers);
        } catch {
            return Results.InternalServerError();
        }
    }

    private static async Task<IResult> GetWeather(DateTime date, IWeatherService weatherService)
    {
        try {
            Weather? weather = await weatherService.GetWeather(date);

            if (weather is null) return Results.NotFound();
            return Results.Ok(weather);
        } catch {
            return Results.InternalServerError();
        }
    }

    private static async Task<IResult> CreateWeather([FromBody] Weather weather, IWeatherService weatherService)
    {
        try {
            Weather? weatherCreated = await weatherService.CreateWeather(weather);
        
            if (weatherCreated is null) return Results.NotFound();
            return Results.Created($"/{weatherCreated?.Id}", weatherCreated);
        } catch {
            return Results.InternalServerError();
        }
    }

    private static async Task<IResult> UpdateWeather(long weatherId, [FromBody] Weather weather, IWeatherService weatherService)
    {
        try {
            Weather? weatherUpdated = await weatherService.UpdateWeather(weatherId, weather);

            if (weatherUpdated is null) return Results.NotFound();
            return Results.Ok(weatherUpdated);
        } catch {
            return Results.InternalServerError();
        }
    }

    private static async Task<IResult> DeleteWeather(long weatherId, IWeatherService weatherService)
    {
        try {
            await weatherService.DeleteWeather(weatherId);
            return Results.Ok();
        } catch {
            return Results.InternalServerError();
        }
    }
}