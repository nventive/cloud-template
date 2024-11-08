using FluentMigrator.Runner;
using Npgsql;
using Placeholder.Migration;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDataSource("weather");

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(runnerBuilder => runnerBuilder
        .AddPostgres()
        .WithGlobalConnectionString(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetConnectionString("weather");
        })
        .WithMigrationsIn(typeof(Worker).Assembly)
        .ScanIn(typeof(Worker).Assembly).For.Migrations())
    .AddLogging(loggerBuilder => loggerBuilder.AddFluentMigratorConsole());

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
