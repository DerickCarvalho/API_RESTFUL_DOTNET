using ApiReservas.Models;
using ApiReservas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherForecastController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public IActionResult GetWeather()
        {
            var weather = _weatherService.GetWeather();
            return Ok(weather);
        }
    }
}
