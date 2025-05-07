using Aspire.Hosting.Azure;
using Azure.Provisioning.PostgreSql;

namespace Placeholder.AppHost
{
    public static class PostgresExtension
    {
        public static IResourceBuilder<AzurePostgresFlexibleServerDatabaseResource> ConfigurePostgresDatabase(this IDistributedApplicationBuilder builder, string databaseName)
        {
            var postgres = builder
                .AddAzurePostgresFlexibleServer("postgres")
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
                        .WithBindMount("../../.pgdata", "/var/lib/postgresql/data") // Persist database
                        .WithPgAdmin();
                });

            return postgres.AddDatabase(databaseName);
        }
    }
}