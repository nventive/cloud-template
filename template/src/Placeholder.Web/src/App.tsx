import { useEffect, useState } from "react";
import "./App.css";
import { Forecast } from "./models/Forecast";

function App() {
  const [forecasts, setForecasts] = useState<Array<Forecast>>([]);

  const apiUrl =  `${process.env.API_BASE_URL}/weathers`

  const requestWeather = async () => {
    const weather = await fetch(apiUrl);
    console.log(weather);

    const weatherJson = await weather.json();
    console.log(weatherJson);

    setForecasts(weatherJson);
  };

  useEffect(() => {
    requestWeather();
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <h1>React (Vite) Weather</h1>
        <table>
          <thead>
            <tr>
              <th>Date</th>
              <th>Temp. (C)</th>
              <th>Summary</th>
            </tr>
          </thead>
          <tbody>
            {(
              forecasts ?? [
                {
                  date: "N/A",
                  temperatureC: "",
                  summary: "No forecasts",
                },
              ]
            ).map((w) => {
              return (
                <tr key={w.date}>
                  <td>{w.date}</td>
                  <td>{w.temperatureC}</td>
                  <td>{w.summary}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </header>
    </div>
  );
}

export default App;
