using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [Route("WeatherForecast")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly IList<string> Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IList<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Count)]
            })
            .ToArray();
        }

        [HttpPost]
        [Authorize(Policy = "edit:weather_forecast")]
        public bool Add(string weatherName)
        {
            if (string.IsNullOrEmpty(weatherName))
                return false;
            Summaries.Add(weatherName);
            return true;
        }
    }
}
