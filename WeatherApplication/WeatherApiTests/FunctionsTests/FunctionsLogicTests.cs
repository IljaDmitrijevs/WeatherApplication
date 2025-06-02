namespace WeatherAppTests.FunctionsTests
{
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Moq.Protected;
    using SharedDbContext.DbData;
    using System.Net;
    using WeatherAppAzureFunctions.Functions;

    public class FunctionsLogicTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly WeatherDbContext _dbContext;
        private readonly Mock<ILogger<FunctionsLogic>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly FunctionsLogic _functionsLogic;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private const string Testkey = "test-api-key";

        public FunctionsLogicTests()
        {
            _dbContext = GetDbContext();

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<FunctionsLogic>>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x["WeatherApiKey"]).Returns(Testkey);

            // HTTP client setup
            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object);
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _functionsLogic = new FunctionsLogic(
                _httpClientFactoryMock.Object,
                _dbContext,
                _loggerMock.Object,
                _configurationMock.Object);
        }

        [Fact]
        public async Task GetsWeatherData_SuccessfulResponse_AddsRecordsToDatabase()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            await _functionsLogic.GetsWeatherData();

            // Assert
            var weatherRecords = _dbContext.WeatherRecords.ToList();
            var weatherLog = _dbContext.WeatherLog;
            weatherRecords.Should().NotBeEmpty();
            weatherLog.Should().NotBeEmpty();
            weatherLog.First().Success.Should().BeTrue();

            weatherRecords[0].City.Should().Be("London");
            weatherRecords[1].City.Should().Be("Rome");
            weatherRecords[2].City.Should().Be("Riga");
        }

        [Fact]
        public async Task GetsWeatherData_FailedResponse_AddsErrorLogToDatabase()
        {
            // Arrange
            var errorMessge = "{\"message\":\"city not found\"}";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(errorMessge),
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            await _functionsLogic.GetsWeatherData();

            // Assert
            _dbContext.WeatherLog.Should().NotBeEmpty();
            _dbContext.WeatherLog.First().Success.Should().BeFalse();
            _dbContext.WeatherLog.First().Message.Should().Contain(errorMessge);
        }

        [Fact]
        public async Task GetsWeatherData_ThrowsException_AddsErrorLogToDatabase()
        {
            // Arrange
            var errorMessge = "Network error";

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException(errorMessge));

            // Act
            await _functionsLogic.GetsWeatherData();

            // Assert
            _dbContext.WeatherLog.Should().NotBeEmpty();
            _dbContext.WeatherLog.First().Success.Should().BeFalse();
            _dbContext.WeatherLog.First().Message.Should().Contain(errorMessge);
        }

        private static WeatherDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new WeatherDbContext(options);

            return context;
        }
    }
}