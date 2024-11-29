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
            SELECT *
            FROM Weather
        ";

        var result = await _dbConnection.QueryAsync<Weather>(query);
        return result.ToArray();
    }

    public async Task<Weather?> GetWeather(long id)
    {
        var query = @"
            SELECT *
            FROM Weather
            WHERE Id = @Id
        ";

        return await _dbConnection.QuerySingleOrDefaultAsync<Weather>(query, new { Id = id });
    }

    public async Task<Weather?> CreateWeather(Weather weather)
    {
        var query = @"
            INSERT INTO Weather (Date, TemperatureC, Summary)
            VALUES (@Date, @TemperatureC, @Summary)
            RETURNING Id, Date, TemperatureC, Summary
        ";

        Weather? weatherCreated = await _dbConnection.QuerySingleOrDefaultAsync<Weather>(query, new {weather.Date, weather.TemperatureC, weather.Summary}) ?? null;
        return weatherCreated;
    }

    public async Task<Weather?> UpdateWeather(long id, Weather weather)
    {
        var query = @"
            UPDATE Weather
            SET Date = @Date, TemperatureC = @TemperatureC, Summary = @Summary
            WHERE Id = @Id
            RETURNING Id, Date, TemperatureC, Summary
        ";

        Weather? weatherUpdated = await _dbConnection.QuerySingleOrDefaultAsync<Weather>(query, new { Id = id, weather.Date, weather.TemperatureC, weather.Summary }) ?? null;
        return weatherUpdated;
    }

    public async Task DeleteWeather(long id)
    {
        var query = @"
            DELETE
            FROM Weather
            WHERE Id = @Id
        ";

        await _dbConnection.ExecuteAsync(query, new { Id = id });
    }
}