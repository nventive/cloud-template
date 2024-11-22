using System.Data.Common;
using Dapper;
using Placeholder.Core.Models;

namespace Placeholder.Core;

internal class WeatherRepository(DbConnection connection) : IWeatherRepository
{
    private readonly DbConnection _dbConnection = connection;

    public async Task<IEnumerable<Weather>> GetWeathers()
    {
        var query = @"
            SELECT Date, TemperatureC, Summary
            FROM Weather
        ";

        var result = await _dbConnection.QueryAsync<Weather>(query);
        return result.ToArray();
    }

    public async Task<Weather?> GetWeather(DateTime date)
    {
        var query = @"
            SELECT Date, TemperatureC, Summary
            FROM Weather
            WHERE Date = @Date
        ";

        return await _dbConnection.QuerySingleOrDefaultAsync<Weather>(query, new { Date = date });
    }

    public async Task<Weather?> CreateWeather(Weather weather)
    {
        var query = @"
            INSERT INTO Weather (Date, TemperatureC, Summary)
            VALUES (@Date, @TemperatureC, @Summary)
            RETURNING Date, TemperatureC, Summary
        ";

        Weather? weatherCreated = await _dbConnection.QuerySingleOrDefaultAsync<Weather>(query, new {weather.Date, weather.TemperatureC, weather.Summary}) ?? null;
        return weatherCreated;
    }

    public async Task<Weather?> UpdateWeather(Weather weather)
    {
        var query = @"
            UPDATE Weather
            SET TemperatureC = @TemperatureC, Summary = @Summary
            WHERE Date = @Date
            RETURNING Date, TemperatureC, Summary
        ";

        Weather? weatherUpdated = await _dbConnection.QuerySingleOrDefaultAsync<Weather>(query, new { weather.Date, weather.TemperatureC, weather.Summary }) ?? null;
        return weatherUpdated;
    }

    public async Task DeleteWeather(DateTime date)
    {
        var query = @"
            DELETE
            FROM Weather
            WHERE Date = @Date
        ";

        await _dbConnection.ExecuteAsync(query, new { Date = date });
    }
}