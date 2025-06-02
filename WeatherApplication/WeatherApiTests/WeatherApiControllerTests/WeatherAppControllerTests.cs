namespace WeatherApiTests.WeatherApiControllerTests
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SharedDbContext.DbData;
    using WeatherAppApi.ApiLogic;
    using WeatherAppApi.Controllers;
    using WeatherAppApi.Interfaces;
    using SharedDbContext.Models;

    public class WeatherControllerTests
    {
        private readonly Mock<IWeatherApiLogic> _mockWeatherLogic;
        private readonly WeatherController _weatherController;
        private readonly WeatherDbContext _dbContext;

        public WeatherControllerTests()
        {
            // Setup in-memory database
            _dbContext = GetDbContextWithData();

            _mockWeatherLogic = new Mock<IWeatherApiLogic>();
            _weatherController = new WeatherController(_dbContext, _mockWeatherLogic.Object);
        }

        [Fact]
        public void GetWeatherData_GetDatawithDateTimeParams_ReturnsResult()
        {
            // Arrange
            _mockWeatherLogic
                .Setup(s => s.GetWeatherData(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<WeatherDbContext>()))
                .Returns(new List<WeatherDto>
                {
                    new WeatherDto
                    { 
                        City = "London", 
                        Timestamp = DateTime.UtcNow,
                    }
                });

            // Act
            var result = _weatherController.GetWeatherData(dateTimeFrom: DateTime.UtcNow, dateTimeTo: DateTime.UtcNow.AddHours(1));

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            result.Should().NotBeNull();
            result.Should().Be(okResult);
        }

        private static WeatherDbContext GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new WeatherDbContext(options);
            context.WeatherRecords.Add(new WeatherRecord
            {
                City = "London",
                Timestamp = DateTime.UtcNow,
                Payload = "{\"main\":{\"temp\":290,\"temp_min\":289,\"temp_max\":291},\"sys\":{\"country\":\"GB\"}}"
            });
            context.SaveChanges();

            return context;
        }
    }
}
