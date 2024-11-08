namespace Placeholder.Migration.Migrations;

using FluentMigrator;

[Migration(20241108112106)]
public class InitialWeatherData : Migration
{
    public override void Up()
    {
        // Create the Weather table
        Execute.Sql(@"
            CREATE TABLE IF NOT EXISTS Weather
            (
                Id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
                Date DATE NOT NULL,
                TemperatureC INT NOT NULL,
                Summary VARCHAR(255)
            );
        ");

        // Insert some test data
        Execute.Sql(@"
            INSERT INTO Weather (Date, TemperatureC, Summary) VALUES
            ('2024-11-01', 15, 'Sunny'),
            ('2024-11-02', 10, 'Cloudy'),
            ('2024-11-03', 5, 'Rainy');
        ");
    }

    public override void Down()
    {
        // Drop the Weather table
        Execute.Sql(@"
            DROP TABLE IF EXISTS Weather;
        ");
    }
}