using Placeholder.Core.Models;

namespace Placeholder.Core;

public interface IWeatherRepository
{
    Task<IEnumerable<Weather>> GetWeathers();
    Task<Weather?> GetWeather(DateTime date);
    Task<Weather?> CreateWeather(Weather weather);
    Task<Weather?> UpdateWeather(long id, Weather weather);
    Task DeleteWeather(long id);
}