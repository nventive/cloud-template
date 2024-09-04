using System.Data.Common;
using Dapper;

namespace Placeholder.Core.Weather;

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public interface IWeatherForecastRepository
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();
}

internal class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly DbConnection _dbConnection;

    public WeatherForecastRepository(DbConnection connection)
    {
        _dbConnection = connection;
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
    {
        var dbResult = await _dbConnection
            .QueryAsync<(DateTime date, int temperatureC, string summary)>(
                "SELECT Date, TemperatureC, Summary FROM Forecasts");

        return dbResult
            .Select(record => new WeatherForecast(DateOnly.FromDateTime(record.date), record.temperatureC, record.summary))
            .ToList();
    }
}