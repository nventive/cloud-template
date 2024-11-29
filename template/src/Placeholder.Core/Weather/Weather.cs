namespace Placeholder.Core.Models;

public class Weather
{
    public Weather() { }

    public Weather(int id, DateTime date, int temperatureC, string? summary)
    {
        Id = id;
        Date = date;
        TemperatureC = temperatureC;
        Summary = summary;
    }

    public int Id { get; set; }
    public DateTime? Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}