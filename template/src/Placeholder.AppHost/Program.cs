var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var weatherDb = postgres.AddDatabase("weather");

var apiService = builder
    .AddProject<Projects.Placeholder_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(weatherDb);

builder.AddProject<Projects.Placeholder_Migration>("migration").WithReference(weatherDb);

builder
    .AddProject<Projects.Placeholder_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
