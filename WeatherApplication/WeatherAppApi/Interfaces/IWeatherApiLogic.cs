namespace WeatherAppApi.Interfaces
{
    using WeatherAppApi.ApiLogic;
    using SharedDbContext.DbData;

    /// <summary>
    /// Weather api logics inteface.
    /// </summary>
    public interface IWeatherApiLogic
    {
        /// <summary>
        /// Gets weather data.
        /// </summary>
        /// <param name="dateTimeFrom">Date time from when started fetching.</param>
        /// <param name="dateTimeTo">Date time to when fetching ended.</param>
        IEnumerable<WeatherDto> GetWeatherData(DateTime? dateTimeFrom, DateTime? dateTimeTo, WeatherDbContext context);

        /// <summary>
        /// Gets latest available weather records timestamp.
        /// </summary>
        /// <param name="context">Db context.</param>
        DateTime? GetLatestWeatherTimestamp(WeatherDbContext context);
    }
}
