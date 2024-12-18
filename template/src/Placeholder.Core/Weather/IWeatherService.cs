using Placeholder.Core.Models;

namespace Placeholder.Core;

public interface IWeatherService
{
    /// <summary>
    /// Get all weathers
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is an enumerable collection of <see cref="Weather"/> objects.
    /// </returns>
    Task<IEnumerable<Weather>> GetWeathers();

    /// <summary>
    /// Get weather by id
    /// </summary>
    /// <param name="id">The database identifier of the weather.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is a <see cref="Weather"/> objects.
    /// If the weather is not found, the task result is <c>null</c>.
    /// </returns>
    Task<Weather?> GetWeather(DateTime date);

    /// <summary>
    /// Create a new weather
    /// </summary>
    /// <param name="weather">The weather object to be added.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is a <see cref="Weather"/> objects.
    /// If the weather is not created, the task result is <c>null</c>.
    /// </returns>
    Task<Weather?> CreateWeather(Weather weather);

    /// <summary>
    /// Update weather by id
    /// </summary>
    /// <param name="id">The database identifier of the weather.</param>
    /// <param name="weather">The weather object to be updated.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is a <see cref="Weather"/> objects with the informations of the updated weather.
    /// If the weather is not found, the task result is <c>null</c>.
    /// </returns>
    Task<Weather?> UpdateWeather(Weather weather);

    /// <summary>
    /// Delete weather by id
    /// </summary>
    /// <param name="id">The database identifier of the weather.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is : Void.
    /// </returns>
    Task DeleteWeather(DateTime date);
}