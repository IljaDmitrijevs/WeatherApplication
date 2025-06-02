namespace WeatherAppAzureFunctions.Functions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using SharedDbContext.DbData;
    using SharedDbContext.Models;
    using WeatherAppAzureFunctions.Constants;
    using WeatherAppAzureFunctions.Interfaces;

    /// <summary>
    /// Functions logic.
    /// </summary>
    public class FunctionsLogic : IFunctionsLogic
    {
        /// <summary>
        /// Http client instance.
        /// </summary>
        private readonly HttpClient _httpClient;
        
        /// <summary>
        /// Weather db context instance.
        /// </summary>
        private readonly WeatherDbContext _dbContext;

        /// <summary>
        /// Logger instance.
        /// </summary>
        private readonly ILogger<FunctionsLogic> _logger;

        /// <summary>
        /// Api key.
        /// </summary>
        private readonly string _apiKey;

        /// <summary>
        /// Functions logic constructor.
        /// </summary>
        /// <param name="httpClientFactory">Client factory.</param>
        /// <param name="dbContext">Db context.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="configuration">Configuration.</param>
        public FunctionsLogic(IHttpClientFactory httpClientFactory, WeatherDbContext dbContext, ILogger<FunctionsLogic> logger, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _dbContext = dbContext;
            _logger = logger;
            _apiKey = configuration["WeatherApiKey"] ?? throw new InvalidOperationException("WeatherApiKey is missing in configuration");
        }

        /// <summary>
        /// Gets weather data with set timer.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public async Task GetsWeatherData()
        {
            _logger.LogInformation("Fetching weather data...");

            foreach (var city in CityList.Cities)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}");
                    var content = await response.Content.ReadAsStringAsync();

                    _dbContext.WeatherRecords.Add(new WeatherRecord
                    {
                        City = city,
                        Payload = content,
                        Timestamp = DateTime.UtcNow,
                    });

                    _dbContext.WeatherLog.Add(new WeatherLog
                    {
                        City = city,
                        Success = response.IsSuccessStatusCode,
                        Message = response.IsSuccessStatusCode ? "Operation was successful" : content,
                        Timestamp = DateTime.UtcNow,
                    });

                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _dbContext.WeatherLog.Add(new WeatherLog
                    {
                        City = city,
                        Success = false,
                        Message = ex.Message,
                        Timestamp = DateTime.UtcNow,
                    });

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
