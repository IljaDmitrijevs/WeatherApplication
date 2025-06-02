namespace WeatherAppApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SharedDbContext.DbData;

    /// <summary>
    /// Weather logs controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        /// <summary>
        /// Db context.
        /// </summary>
        private readonly WeatherDbContext _context;

        /// <summary>
        /// Weather logs controllert constructor.
        /// </summary>
        /// <param name="context">Db context</param>
        public LogsController(WeatherDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets weather logs.
        /// </summary>
        [HttpGet]
        public IActionResult GetLogs()
        {
            var logs = _context.WeatherLog
                .OrderByDescending(x => x.Timestamp)
                .Select(x => new
                {
                    x.City,
                    x.Success,
                    x.Message,
                    x.Timestamp,
                }) 
                .ToList();

            var updatedLogs = logs
    .           Select(log => log with { Timestamp = log.Timestamp.ToLocalTime() })
                .ToList();


            return Ok(updatedLogs);
        }
    }
}
