using Placeholder.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var weatherDb = builder.ConfigurePostgresDatabase("weather", builder.AddParameter("postgresUsername"), builder.AddParameter("postgresPassword", secret: true));

// Use the following for automatic provisioning:
var appInsights = builder.AddAzureApplicationInsights("appinsights");
// Use the following for manual provisioning and set a user secret for "ConnectionStrings:AppInsights"
//var appInsights = builder.AddConnectionString("AppInsights", "APPLICATIONINSIGHTS_CONNECTION_STRING");

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
