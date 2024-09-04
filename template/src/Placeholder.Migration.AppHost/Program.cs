var builder = DistributedApplication.CreateBuilder(args);

// NB: the database setup is the same as in the Placeholder.AppHost project.
const string weatherDbName = "weather";

var usernameParameter = builder.AddParameter("postgresUsername", secret: true);
var passwordParameter = builder.AddParameter("postgresPassword", secret: true);

var postgres = builder
    .AddPostgres("postgres", userName: usernameParameter, password: passwordParameter)
    .WithEnvironment("POSTGRES_DB", weatherDbName)
    .WithPgAdmin()
    .WithBindMount("../data", "/docker-entrypoint-initdb.d")
    .WithBindMount("../../.data", "/var/lib/postgresql/data"); // Persist database

var weatherDb = postgres.AddDatabase(weatherDbName);

builder.AddProject<Projects.Placeholder_Migration>("migration")
    .WithReference(weatherDb);

builder.Build().Run();
