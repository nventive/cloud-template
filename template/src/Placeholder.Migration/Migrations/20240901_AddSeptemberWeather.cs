namespace Placeholder.Migration.Migrations;

using FluentMigrator;

[Migration(20240901000000)]
public class AddSeptemberWeather : Migration
{
    public override void Up()
    {
        Insert.IntoTable("forecasts")
            .Row(new { date = "2024-09-01", temperaturec = 20, summary = "Sunny" })
            .Row(new { date = "2024-09-02", temperaturec = 22, summary = "Rainy" })
            .Row(new { date = "2024-09-03", temperaturec = 24, summary = "Windy" });
    }

    public override void Down()
    {
        // Nothing
    }
}