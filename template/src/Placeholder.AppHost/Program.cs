var builder = DistributedApplication.CreateBuilder(args);

const string weatherDbName = "weather";

var postgres = builder.AddPostgres("postgres")
    .WithEnvironment("POSTGRES_DB", weatherDbName)
    .WithPgAdmin()
    .WithBindMount("../data", "/docker-entrypoint-initdb.d")
    .WithBindMount("../../.data", "/var/lib/postgresql/data"); // Persist database

var weatherDb = postgres.AddDatabase(weatherDbName);

var apiService = builder.AddProject<Projects.Placeholder_ApiService>("apiservice")
    .WithReference(weatherDb);

builder.AddProject<Projects.Placeholder_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
