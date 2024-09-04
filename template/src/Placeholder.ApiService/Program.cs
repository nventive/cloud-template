using Microsoft.AspNetCore.Mvc;
using Placeholder.Core.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddPlaceholderCoreServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/weatherforecast", ([FromServices] IWeatherForecastRepository repository) =>
{
    return repository.GetWeatherForecasts();
});

app.MapDefaultEndpoints();

app.Run();
