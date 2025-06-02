namespace WeatherApiTests.WeatherApiControllerTests
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SharedDbContext.DbData;
    using WeatherAppApi.Controllers;
    using SharedDbContext.Models;

    public class WeatherLogsControllerTests
    {
        [Fact]
        public void WeatherLogsCDontroller_GetLogData_ReturnsLogs()
        {
            var context = GetDbContextWithData();
            var controller = new LogsController(context);

            var result = controller.GetLogs();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var logs = Assert.IsAssignableFrom<System.Collections.IEnumerable>(okResult.Value);
            logs.Should().NotBeNull();
        }

        private static WeatherDbContext GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new WeatherDbContext(options);
            context.WeatherLog.Add(new WeatherLog
            {
                City = "Rome",
                Success = true,
                Message = "OK",
                Timestamp = DateTime.UtcNow
            });
            context.SaveChanges();

            return context;
        }
    }
}
