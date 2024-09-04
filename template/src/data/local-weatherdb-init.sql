-- Postgres init script

CREATE TABLE IF NOT EXISTS Forecasts
(
    Date date PRIMARY KEY,
    TemperatureC int,
    Summary text
);

INSERT INTO Forecasts(Date, TemperatureC, Summary)
VALUES
    ('2024-09-01', 20, 'Sunny'),
    ('2024-09-02', 18, 'Rainy'),
    ('2024-09-03', 22, 'Windy')
ON CONFLICT DO NOTHING;