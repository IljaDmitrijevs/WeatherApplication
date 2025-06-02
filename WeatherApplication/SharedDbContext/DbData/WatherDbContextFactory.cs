namespace WeatherAppAzureFunctions.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using SharedDbContext.DbData;

    /// <summary>
    /// Weather db context factory.
    /// </summary>
    public class WeatherDbContextFactory : IDesignTimeDbContextFactory<WeatherDbContext>
    {
        /// <summary>
        /// Creates db context.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public WeatherDbContext CreateDbContext(string[] args)
        {
            // Manually build configuration
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            // Get the connection string from config
            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WeatherDbContext(optionsBuilder.Options);
        }
    }
}