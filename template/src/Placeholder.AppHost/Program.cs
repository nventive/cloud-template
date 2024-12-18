using Placeholder.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var weatherDb = builder.ConfigurePostgresDatabase("weather", builder.AddParameter("postgresUsername"), builder.AddParameter("postgresPassword", secret: true));

var apiService = builder.AddProject<Projects.Placeholder_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(weatherDb);

var migration = builder.AddProject<Projects.Placeholder_Migration>("migration")
    .WithReference(weatherDb);

var webfrontend = builder.AddProject<Projects.Placeholder_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

#if appInsights
// Use the following for automatic provisioning:
var appInsights = builder.AddAzureApplicationInsights("appinsights");
// Use the following for manual provisioning and set a user secret for "ConnectionStrings:AppInsights"
//var appInsights = builder.AddConnectionString("AppInsights", "APPLICATIONINSIGHTS_CONNECTION_STRING");
apiService.WithReference(appInsights);
migration.WithReference(appInsights);
webfrontend.WithReference(appInsights);
#endif

builder.Build().Run();
