namespace WeatherAppAzureFunctions.Functions
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using WeatherAppAzureFunctions.Interfaces;

    /// <summary>
    /// Fect weather data function.
    /// </summary>
    public class FetchWeatherDataFunction
    {
        /// <summary>
        /// Functions logic instance.
        /// </summary>
        private readonly IFunctionsLogic _logic;

        /// <summary>
        /// Logger instance.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Exception meessage which will be thrown.
        /// </summary>
        private const string ExceptionMessage = "Something went wrong";

        /// <summary>
        /// Fect weather data function constructor.
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="loggerFactory"></param>
        public FetchWeatherDataFunction(IFunctionsLogic logic, ILoggerFactory loggerFactory)
        {
            _logic = logic;
            _logger = loggerFactory.CreateLogger<FetchWeatherDataFunction>();
        }

        /// <summary>
        /// Runs timer function.
        /// </summary>
        /// <param name="timerInfo">Timer information.</param>
        /// <exception cref="Exception">Exception</exception>
        [Function("FetchWeatherData")]
        public async Task RunTimer([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo)
        {
            try
            {
                _logger.LogInformation($"Timer trigger function executed at: {DateTime.UtcNow}");
                await _logic.GetsWeatherData();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Caught exception: {ex}");
                throw new Exception(ExceptionMessage);
            }
        }
    }
}