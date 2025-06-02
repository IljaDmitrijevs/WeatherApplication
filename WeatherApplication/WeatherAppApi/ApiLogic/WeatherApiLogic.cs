namespace WeatherAppApi.ApiLogic
{
    using System.Text.Json;
    using SharedDbContext.DbData;
    using WeatherAppApi.Interfaces;

    /// <summary>
    /// Weather api logic.
    /// </summary>
    public class WeatherApiLogic : IWeatherApiLogic
    {
        /// <summary>
        /// Gets weather data.
        /// </summary>
        /// <param name="dateTimeFrom">Date time from when started fetching.</param>
        /// <param name="dateTimeTo">Date time to when fetching ended.</param>
        /// <returns></returns>
        public IEnumerable<WeatherDto> GetWeatherData(DateTime? dateTimeFrom, DateTime? dateTimeTo, WeatherDbContext context) 
        {
            var query = context.WeatherRecords.AsQueryable();

            if (dateTimeFrom.HasValue)
            {
                query = query.Where(w => w.Timestamp >= dateTimeFrom.Value);
            }

            if (dateTimeTo.HasValue)
            {
                query = query.Where(w => w.Timestamp <= dateTimeTo.Value);
            }

            var records = query
                .OrderByDescending(w => w.Timestamp)
                .ToList();

            return records.Select(w => new WeatherDto
            {
                City = w.City,
                Timestamp = w.Timestamp.ToLocalTime(),
                Payload = JsonSerializer.Deserialize<JsonElement>(w.Payload)
            });
        }

        /// <summary>
        /// Gets latest available weather records timestamp.
        /// </summary>
        /// <param name="context">Db context.</param>
        public DateTime? GetLatestWeatherTimestamp(WeatherDbContext context)
        {
            var latest = context.WeatherRecords
                .OrderByDescending(w => w.Timestamp)
                .FirstOrDefault();

            return latest?.Timestamp.ToLocalTime();
        }
    }
}
