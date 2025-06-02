namespace SharedDbContext.DbData
{
    using Microsoft.EntityFrameworkCore;
    using SharedDbContext.Models;

    /// <summary>
    /// Weather db context.
    /// </summary>
    /// <param name="options">Options.</param>
    public class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
    {
        public DbSet<WeatherRecord> WeatherRecords { get; set; }
        public DbSet<WeatherLog> WeatherLog { get; set; }
    }
}