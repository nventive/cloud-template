using Placeholder.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var weatherDb = builder.ConfigurePostgresDatabase("weather", builder.AddParameter("postgresUsername"), builder.AddParameter("postgresPassword", secret: true));

var appInsights = builder.AddConnectionString("AppInsights", "APPLICATIONINSIGHTS_CONNECTION_STRING");

var apiService = builder.AddProject<Projects.Placeholder_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(weatherDb)
    .WithReference(appInsights);

builder.AddProject<Projects.Placeholder_Migration>("migration")
    .WithReference(weatherDb)
    .WithReference(appInsights);

builder.AddProject<Projects.Placeholder_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(appInsights);

builder.Build().Run();
