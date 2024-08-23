var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Placeholder_ApiService>("apiservice");

builder.AddProject<Projects.Placeholder_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
