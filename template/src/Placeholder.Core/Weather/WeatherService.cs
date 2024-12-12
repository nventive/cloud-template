using Placeholder.Core.Models;

namespace Placeholder.Core;

internal class WeatherService(IWeatherRepository weatherRepository) : IWeatherService
{
    private readonly IWeatherRepository _weatherRepository = weatherRepository;

    public async Task<IEnumerable<Weather>> GetWeathers()
    {
        return await _weatherRepository.GetWeathers();
    }

    public async Task<Weather?> GetWeather(DateTime date)
    {
        return await _weatherRepository.GetWeather(date);
    }

    public async Task<Weather?> CreateWeather(Weather weather)
    {
        return await _weatherRepository.CreateWeather(weather);
    }

    public async Task<Weather?> UpdateWeather(long id, Weather weather)
    {
        return await _weatherRepository.UpdateWeather(id, weather);
    }

    public async Task DeleteWeather(long id)
    {
        await _weatherRepository.DeleteWeather(id);
    }
}