namespace WeatherAppAzureFunctions.Interfaces
{
    /// <summary>
    /// Functions logic interface.
    /// </summary>
    public interface IFunctionsLogic
    {
        /// <summary>
        /// Gets weather data with set timer.
        /// </summary>
        Task GetsWeatherData();
    }
}
