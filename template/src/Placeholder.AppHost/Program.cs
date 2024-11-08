using Azure.Provisioning.PostgreSql;

var builder = DistributedApplication.CreateBuilder(args);

const string contestDbName = "weather";

var usernameParameter = builder.AddParameter("postgresUsername");
var passwordParameter = builder.AddParameter("postgresPassword", secret: true);

var postgres = builder
    .AddAzurePostgresFlexibleServer("postgres")
    // TODO: try to find a way to get fluent migrator to work with a connection directly
    .WithPasswordAuthentication(usernameParameter, passwordParameter)
    // TODO: Replace this with .ConfigureInfrastructure in Aspire 9.0 RTM
    .ConfigureConstruct(construct =>
    {
        var postgresServer = construct
            .GetResources()
            .OfType<PostgreSqlFlexibleServer>()
            .Single();

        postgresServer.AvailabilityZone = "";
    })
    .RunAsContainer(postgresBuilder =>
    {
        postgresBuilder
            .WithEnvironment("POSTGRES_DB", contestDbName)
            .WithBindMount("../data", "/docker-entrypoint-initdb.d")
            .WithBindMount("../../.data", "/var/lib/postgresql/data") // Persist database
            .WithPgAdmin()
            ;
    })
    ;

var contestDb = postgres.AddDatabase(contestDbName);

var apiService = builder.AddProject<Projects.Placeholder_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(contestDb);


builder.AddProject<Projects.Placeholder_Migration>("migration")
    .WithReference(contestDb);

builder.AddProject<Projects.Placeholder_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
