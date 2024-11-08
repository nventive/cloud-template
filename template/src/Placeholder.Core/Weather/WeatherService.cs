using Placeholder.Core;
using Placeholder.Core.Models;

namespace Placeholder.Core;

internal class WeatherService : IWeatherService
{
    private readonly IWeatherRepository _weatherRepository;
    public WeatherService(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }

    public async Task<Weather[]> GetWeather()
    {
        return await _weatherRepository.GetWeather();
    }
}