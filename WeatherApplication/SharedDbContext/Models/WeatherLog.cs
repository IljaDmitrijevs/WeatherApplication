namespace SharedDbContext.Models
{
    /// <summary>
    /// Weather log model.
    /// </summary>
    public class WeatherLog
    {
        /// <summary>
        /// Weather record log id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Weather record log city.
        /// </summary>
        public required string City { get; set; }

        /// <summary>
        /// Indicates wheter weather fetching was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Log message.
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// Weather record log date time.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
