using Placeholder.Core.Models;

namespace Placeholder.Core;

public interface IWeatherService
{
    Task<Weather[]> GetWeathers();
    Task<Weather?> GetWeather(long id);
    Task<Weather?> CreateWeather(Weather weather);
    Task<Weather?> UpdateWeather(long id, Weather weather);
    Task DeleteWeather(long id);
}