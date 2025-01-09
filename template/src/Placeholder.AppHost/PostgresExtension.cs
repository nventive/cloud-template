using Aspire.Hosting.Azure;
using Azure.Provisioning.PostgreSql;

namespace Placeholder.AppHost
{
    public static class PostgresExtension
    {
        public static IResourceBuilder<AzurePostgresFlexibleServerDatabaseResource> ConfigurePostgresDatabase(this IDistributedApplicationBuilder builder, string databaseName, IResourceBuilder<ParameterResource>? username, IResourceBuilder<ParameterResource>? password)
        {
            var postgres = builder
                .AddAzurePostgresFlexibleServer("postgres")
                .WithPasswordAuthentication(username, password)
                .ConfigureInfrastructure(construct =>
                {
                    var postgresServer = construct
                        .GetProvisionableResources()
                        .OfType<PostgreSqlFlexibleServer>()
                        .Single();

                    postgresServer.AvailabilityZone = "";
                })
                .RunAsContainer(postgresBuilder =>
                {
                    postgresBuilder
                        .WithEnvironment("POSTGRES_DB", databaseName)
                        .WithBindMount("../data", "/docker-entrypoint-initdb.d")
                        .WithBindMount("../../.data", "/var/lib/postgresql/data") // Persist database
                        .WithPgAdmin();
                });

            return postgres.AddDatabase(databaseName);
        }
    }
}