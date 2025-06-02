using System.Text.Json;

namespace WeatherAppApi.ApiLogic
{
    /// <summary>
    /// Weather dto.
    /// </summary>
    public class WeatherDto
    {
        /// <summary>
        /// City from where temperature info is gathered.
        /// </summary>
        public required string City { get; set; }

        /// <summary>
        /// Timestamp when data was gathered.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Payload of gathered data.
        /// </summary>
        public JsonElement Payload { get; set; }
    }

}
