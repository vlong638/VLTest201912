using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace VL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public WeatherForecast GetOne()
        {
            var rng = new Random();
            return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> GetSome()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public int GetInt()
        {
            return 1;
        }

        [HttpGet]
        public List<int> GetInts()
        {
            return new List<int>() { 1, 2, 3 };
        }

        [HttpGet]
        public string GetString()
        {
            return "s123";
        }

        [HttpGet]
        public List<string> GetStrings()
        {
            return new List<string>() { "s1", "s2", "s3" };
        }

        [HttpGet]
        public Dictionary<string, int> GetDictionary()
        {
            return new Dictionary<string, int>() { { "a", 1 }, { "b", 2 } };
        }
    }
}
