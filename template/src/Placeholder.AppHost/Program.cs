using Placeholder.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var weatherDb = builder.ConfigurePostgresDatabase("weather", builder.AddParameter("postgresUsername"), builder.AddParameter("postgresPassword", secret: true));

var blobStorage = builder.AddAzureStorage("storage")
    .RunAsEmulator()
    .AddBlobs("images");

var apiService = builder
    .AddProject<Projects.Placeholder_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(weatherDb)
    .WithReference(blobStorage);

var migration = builder.AddProject<Projects.Placeholder_Migration>("migration")
    .WithReference(weatherDb)
    .WithExplicitStart();

var webfrontend = builder.AddProject<Projects.Placeholder_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

#if appInsights
if (builder.ExecutionContext.IsPublishMode)
{
    var appInsights = builder.AddAzureApplicationInsights("appinsights");

    apiService.WithReference(appInsights);
    migration.WithReference(appInsights);
    webfrontend.WithReference(appInsights);
}
#endif

builder.Build().Run();
