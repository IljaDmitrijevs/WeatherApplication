namespace WeatherAppApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SharedDbContext.DbData;
    using WeatherAppApi.Interfaces;

    /// <summary>
    /// Weather controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        /// <summary>
        /// Db context instance.
        /// </summary>
        private readonly WeatherDbContext _context;

        /// <summary>
        /// Weather api logic instance.
        /// </summary>
        private readonly IWeatherApiLogic _weatherApiLogic;

        /// <summary>
        /// Weahter controller constructor.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="weatherApiLogic">Weather api logic.</param>
        public WeatherController(WeatherDbContext context, IWeatherApiLogic weatherApiLogic)
        {
            _context = context;
            _weatherApiLogic = weatherApiLogic;
        }

        /// <summary>
        /// Gets weather data information.
        /// </summary>
        /// <param name="dateTimeFrom">Date time from when started fetching.</param>
        /// <param name="dateTimeTo">Date time to when fetching ended.</param>
        [HttpGet]
        public IActionResult GetWeatherData([FromQuery] DateTime? dateTimeFrom, [FromQuery] DateTime? dateTimeTo)
        {
            var defaultFrom = DateTime.UtcNow.AddHours(-12);
            var result = _weatherApiLogic.GetWeatherData(dateTimeFrom ?? defaultFrom, dateTimeTo, _context);

            return Ok(result);
        }
        /// <summary>
        /// Gets latest weather record timestamp.
        /// </summary>
        [HttpGet("latest-timestamp")]
        public IActionResult GetLatestWeatherTimestamp()
        {
            var latestTimestamp = _weatherApiLogic.GetLatestWeatherTimestamp(_context);

            if (latestTimestamp == null)
            {
                return NotFound("No weather data available.");
            }

            return Ok(new { timestamp = latestTimestamp });
        }
    }
}
