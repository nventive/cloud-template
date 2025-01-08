using Placeholder.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var weatherDb = builder.ConfigurePostgresDatabase("weather", builder.AddParameter("postgresUsername"), builder.AddParameter("postgresPassword", secret: true));

var storage = builder.AddAzureStorage("storage");
if (builder.ExecutionContext.IsRunMode)
{
    storage.RunAsEmulator();
}
var blobs = storage.AddBlobs("blobs");

var apiService = builder.AddProject<Projects.Placeholder_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(weatherDb)
    .WithReference(blobs);

builder.AddProject<Projects.Placeholder_Migration>("migration")
    .WithReference(weatherDb);

builder.AddProject<Projects.Placeholder_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
