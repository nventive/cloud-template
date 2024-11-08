namespace Placeholder.Core.Models;

public class Weather
{
    public Weather() { }

    public Weather(string date, int temperatureC, string? summary)
    {
        Date = date;
        TemperatureC = temperatureC;
        Summary = summary;
    }

    public string? Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}