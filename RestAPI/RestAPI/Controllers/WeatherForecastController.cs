using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;
using System.IO;
using System.Reflection.PortableExecutable;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme)]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Api executing");
           // _logger.LogError("Api executing");
            StringValues headers;
            if (HttpContext.Request.Headers.TryGetValue("X-ARR-ClientCert", out headers))
            {
                string header = HttpContext.Request.Headers["X-ARR-ClientCert"];

                
                _logger.LogInformation("Cert " + header);
            }
            else
            {
                _logger.LogInformation("No Cert");
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
}