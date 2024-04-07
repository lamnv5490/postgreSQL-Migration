using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PostgreSQL_Migration.APIs.Infras;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostgreSQL_Migration.APIs.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<WeatherForecastController> _logger;

    readonly IEventFactory _eventFactory;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IEventFactory eventFactory)
    {
        _logger = logger;

        _eventFactory = eventFactory;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var myEvent = "CreateOrderEvent";

        if (_eventFactory.GetEventType(myEvent) is { } eventType)
        {
            foreach (var h in _eventFactory.GetHandlers(eventType))
            {
                if (h?.GetType().GetMethod(nameof(IEventHandler<IEvent>.Handle)) is { } method)
                {
                    _ = method.Invoke(h, new[] { myEvent })!;
                }
            }
        }
        else
        {
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}