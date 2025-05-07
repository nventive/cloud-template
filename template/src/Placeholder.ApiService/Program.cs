using Placeholder.ApiService.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddCoreServices();

// CORS open to web front
builder.Services.AddCors(corsOptions =>
    corsOptions.AddDefaultPolicy(corsPolicy => {
        if (builder.Configuration["services:webfrontend:http:0"] is string httpUrl)
            corsPolicy.WithOrigins(httpUrl);
        if (builder.Configuration["services:webfrontend:https:0"] is string httpsUrl)
            corsPolicy.WithOrigins(httpsUrl);

        corsPolicy
            .AllowAnyMethod()
            .AllowAnyHeader();
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors();

app.ConfigureOpenApi();
app.MapWeatherEndpoints();
app.MapBlobDemoEndpoints();
app.MapDefaultEndpoints();

app.Run();
