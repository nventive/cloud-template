var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var weatherDb = postgres.AddDatabase("weather");

var blobStorage = builder.AddAzureStorage("storage").AddBlobs("images");

var apiService = builder
    .AddProject<Projects.Placeholder_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(weatherDb)
    .WithReference(blobStorage);

var migration = builder.AddProject<Projects.Placeholder_Migration>("migration")
    .WithReference(weatherDb);

var webfrontend = builder.AddProject<Projects.Placeholder_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

#if appInsights
var appInsights = builder.AddAzureApplicationInsights("appinsights");

apiService.WithReference(appInsights);
migration.WithReference(appInsights);
webfrontend.WithReference(appInsights);
#endif

builder.Build().Run();
