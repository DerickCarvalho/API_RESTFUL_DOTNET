using ApiReservas.Models;

namespace ApiReservas.Services
{
    public class WeatherService
    {
        private static readonly string[] Summaries = { "Frio", "Quente", "Agradável", "Chuvoso" };

        public WeatherForecast GetWeather()
        {
            var rng = new Random();
            return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = rng.Next(-10, 35),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };
        }
    }
}
