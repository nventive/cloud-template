using Placeholder.Core.Models;

namespace Placeholder.Core;

public interface IWeatherService
{
    Task<Weather[]> GetWeather();
}