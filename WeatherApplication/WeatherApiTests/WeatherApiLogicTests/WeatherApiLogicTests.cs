namespace WeatherApiTests.WeatherApiLogicTests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using SharedDbContext.DbData;
    using WeatherAppApi.ApiLogic;
    using SharedDbContext.Models;
    using Xunit;

    public class WeatherApiLogicTests
    {
        private readonly WeatherDbContext _context;
        private readonly WeatherApiLogic _logic;

        public WeatherApiLogicTests()
        {
            _context = GetDbContextWithData();
            _logic = new WeatherApiLogic();
        }

        [Fact]
        public void GetWeatherData_NoFilter_ReturnsAllRecords()
        {
            // Act
            var result = _logic.GetWeatherData(null, null, _context).ToList();

            // Assert
            result.Should().NotBeEmpty();
            result.Count.Should().Be(3);
            result[0].City.Should().Be("Riga");
            result[1].City.Should().Be("Rome");
            result[2].City.Should().Be("London");
        }

        [Fact]
        public void GetWeatherData_FilteredByFromDate_ReturnsAccorindgResult()
        {
            // Arrange
            var fromDate = DateTime.UtcNow.AddDays(-1);

            // Act
            var result = _logic.GetWeatherData(fromDate, null, _context).ToList();

            // Assert
            result.Count.Should().Be(2);
        }

        [Fact]
        public void GetWeatherData_FilerByToDate_ReturnsAccoringResult()
        {
            // Arrange
            var toDate = DateTime.UtcNow.AddDays(-2);

            // Act
            var result = _logic.GetWeatherData(null, toDate, _context).ToList();

            // Assert
            result.Count.Should().Be(1);
        }

        private static WeatherDbContext GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new WeatherDbContext(options);

            context.WeatherRecords.AddRange(
                new WeatherRecord
                {
                    City = "London",
                    Timestamp = DateTime.UtcNow.AddDays(-2),
                    Payload = "{\"main\":{\"temp\":280,\"temp_min\":275,\"temp_max\":285},\"sys\":{\"country\":\"London\"}}"
                },
                new WeatherRecord
                {
                    City = "Rome",
                    Timestamp = DateTime.UtcNow.AddHours(-3),
                    Payload = "{\"main\":{\"temp\":290,\"temp_min\":288,\"temp_max\":292},\"sys\":{\"country\":\"Rome\"}}"
                },
                new WeatherRecord
                {
                    City = "Riga",
                    Timestamp = DateTime.UtcNow,
                    Payload = "{\"main\":{\"temp\":278,\"temp_min\":276,\"temp_max\":280},\"sys\":{\"country\":\"Riga \"}}"
                });

            context.SaveChanges();
            return context;
        }
    }
}
