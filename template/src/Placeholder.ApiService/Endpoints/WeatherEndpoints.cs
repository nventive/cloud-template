using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Placeholder.Core;
using Placeholder.Core.Models;

namespace Placeholder.ApiService.Endpoints;
public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var weatherGroup = endpoints.MapGroup("/weather");

        endpoints.MapGet("/weathers", GetAllWeathers);
        weatherGroup.MapGet("/{date}", GetWeather);
        weatherGroup.MapPost("/", CreateWeather);
        weatherGroup.MapPut("/", UpdateWeather);
        weatherGroup.MapDelete("/{date}", DeleteWeather);
    }

    private static async Task<Results<Ok<IEnumerable<Weather>>, InternalServerError>> GetAllWeathers(IWeatherService weatherService)
    {
        try {
            IEnumerable<Weather> weathers = await weatherService.GetWeathers();
            return TypedResults.Ok(weathers);
        } catch {
            return TypedResults.InternalServerError();
        }
    }

    private static async Task<Results<Ok<Weather>, NotFound, InternalServerError>> GetWeather(DateTime date, IWeatherService weatherService)
    {
        try {
            Weather? weather = await weatherService.GetWeather(date);

            if (weather is null) return TypedResults.NotFound();
            return TypedResults.Ok(weather);
        } catch {
            return TypedResults.InternalServerError();
        }
    }

    private static async Task<Results<Created<Weather>, NotFound, InternalServerError>> CreateWeather([FromBody] Weather weather, IWeatherService weatherService)
    {
        try {
            Weather? weatherCreated = await weatherService.CreateWeather(weather);
        
            return TypedResults.Created($"weather/{weatherCreated?.Date:O}", weatherCreated);
        } catch {
            return TypedResults.InternalServerError();
        }
    }

    private static async Task<Results<Ok<Weather>, NotFound, InternalServerError>> UpdateWeather([FromBody] Weather weather, IWeatherService weatherService)
    {
        try {
            Weather? weatherUpdated = await weatherService.UpdateWeather(weather);

            if (weatherUpdated is null) return TypedResults.NotFound();
            return TypedResults.Ok(weatherUpdated);
        } catch {
            return TypedResults.InternalServerError();
        }
    }

    private static async Task<Results<Ok, InternalServerError>> DeleteWeather(DateTime date, IWeatherService weatherService)
    {
        try {
            await weatherService.DeleteWeather(date);
            return TypedResults.Ok();
        } catch {
            return TypedResults.InternalServerError();
        }
    }
}