namespace WeatherAppTests.FunctionsTests
{
    using FluentAssertions;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using Moq;
    using WeatherAppAzureFunctions.Functions;
    using WeatherAppAzureFunctions.Interfaces;

    public class FetchWeatherDataFunctionTests
    {
        private readonly Mock<IFunctionsLogic> _functionsLogicMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<ILogger<FetchWeatherDataFunction>> _loggerMock;
        private readonly FetchWeatherDataFunction _function;

        public FetchWeatherDataFunctionTests()
        {
            _functionsLogicMock = new Mock<IFunctionsLogic>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _loggerMock = new Mock<ILogger<FetchWeatherDataFunction>>();
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);

            _function = new FetchWeatherDataFunction(
                _functionsLogicMock.Object,
                _loggerFactoryMock.Object);
        }

        [Fact]
        public async Task RunTimer_InvokesGetsWeatherData_GetsWeatherDataLogicCalled()
        {
            // Arrange
            var timerInfoMock = new Mock<TimerInfo>();

            // Act
            await _function.RunTimer(timerInfoMock.Object);

            // Assert
            _functionsLogicMock.Verify(x => x.GetsWeatherData(), Times.Once);
            _loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Timer trigger function executed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        // Add
        [Fact]
        public async Task Run_WeatherDataThrowsException()
        {
            // Arrange
            var exceptionMessage = "Something went wrong";
            var timerInfoMock = new Mock<TimerInfo>();

            _functionsLogicMock.Setup(x => x.GetsWeatherData())
                .ThrowsAsync(new Exception());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _function.RunTimer(timerInfoMock.Object));

            exception.Message.Should().BeEquivalentTo(exceptionMessage);
        }
    }
}