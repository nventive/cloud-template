using Placeholder.Core.Models;

namespace Placeholder.Core;

public interface IWeatherService
{
    /// <summary>
    /// Get all weathers
    /// </summary>
    Task<Weather[]> GetWeathers();

    /// <summary>
    /// Get weather by id
    /// </summary>
    Task<Weather?> GetWeather(long id);

    /// <summary>
    /// Create a new weather
    /// </summary>
    Task<Weather?> CreateWeather(Weather weather);

    /// <summary>
    /// Update weather by id
    /// </summary>
    Task<Weather?> UpdateWeather(long id, Weather weather);

    /// <summary>
    /// Delete weather by id
    /// </summary>
    Task DeleteWeather(long id);
}