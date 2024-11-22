namespace Placeholder.Core.Models;

public class Weather
{
    public Weather() { }

    public Weather(DateTime date, int temperatureC, string? summary)
    {
        Date = date;
        TemperatureC = temperatureC;
        Summary = summary;
    }

    public DateTime? Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}