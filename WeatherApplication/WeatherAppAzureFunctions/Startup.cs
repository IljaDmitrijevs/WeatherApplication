using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedDbContext.DbData;
using WeatherAppAzureFunctions.Functions;
using WeatherAppAzureFunctions.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((ctx, services) =>
    {
        var config = ctx.Configuration;
        var connStr = config.GetConnectionString("SqlConnection");

        services.AddHttpClient();
        services.AddDbContext<WeatherDbContext>(options =>
            options.UseSqlServer(connStr));

        services.AddScoped<IFunctionsLogic, FunctionsLogic>();
    })
    .Build();
host.Run();
