using Placeholder.ApiService.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddCoreServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.ConfigureOpenApi();
app.MapWeatherEndpoints();
app.MapDefaultEndpoints();

app.Run();
