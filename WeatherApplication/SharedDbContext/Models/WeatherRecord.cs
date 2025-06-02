namespace SharedDbContext.Models
{
    /// <summary>
    /// Weather record model.
    /// </summary>
    public class WeatherRecord
    {
        /// <summary>
        /// Weather record id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Weather record city.
        /// </summary>
        public required string City { get; set; }

        /// <summary>
        /// Weather record payload.
        /// </summary>
        public required string Payload { get; set; }

        /// <summary>
        /// Weather record date time.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
