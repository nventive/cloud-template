using System.Data.Common;
using Dapper;
using Placeholder.Core.Models;

namespace Placeholder.Core;

internal class WeatherRepository : IWeatherRepository
{
    private readonly DbConnection _dbConnection;

    public WeatherRepository(DbConnection connection)
    {
        _dbConnection = connection;
    }

    public async Task<Weather[]> GetWeather()
    {
        var query = @"
            SELECT Date, TemperatureC, Summary
            FROM Weather
        ";

        var result = await _dbConnection.QueryAsync<Weather>(query);
        return result.ToArray();
    }
}