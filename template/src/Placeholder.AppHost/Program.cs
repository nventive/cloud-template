using Microsoft.Extensions.Hosting;
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
    .WithReference(blobStorage)
    ;

var migration = builder.AddProject<Projects.Placeholder_Migration>("migration")
    .WithReference(weatherDb)
    .WithExplicitStart();

var nodeApp = new NodeAppResource(
    "webfrontend", 
    "yarn", // Change command to yarn
    Path.Combine(builder.AppHostDirectory, "../Placeholder.Web")); 

var webfrontend = builder.AddResource(nodeApp)
    .WithArgs("run", "start")
    .WithOtlpExporter()
    .WithEnvironment("NODE_ENV", builder.Environment.EnvironmentName)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

if (builder.ExecutionContext.IsPublishMode)
{
    webfrontend.WithHttpEndpoint(env: "VITE_PORT");
}
else
{
    // This port is also specified in the launch.json.
    webfrontend.WithHttpEndpoint(port: 7123, env: "VITE_PORT");
}

apiService.WithReference(webfrontend); // Necessary to setup CORS rule.

#if appInsights
if (builder.ExecutionContext.IsPublishMode)
{
    var appInsights = builder.AddAzureApplicationInsights("appinsights");

    apiService.WithReference(appInsights);
    migration.WithReference(appInsights);
}
#endif

builder.Build().Run();
