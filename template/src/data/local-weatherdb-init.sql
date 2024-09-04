-- Postgres init script

CREATE TABLE IF NOT EXISTS Forecasts
(
    Date date PRIMARY KEY,
    TemperatureC int,
    Summary text
);
