using Placeholder.Core.Models;

namespace Placeholder.Core;

public interface IWeatherRepository
{
    Task<Weather[]> GetWeather();
}