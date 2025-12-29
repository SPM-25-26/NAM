using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Infrastructure.Extensions
{
    public static class SerilogConfigurator
    {
        public static LoggerConfiguration ApplySharedLoggerConfiguration(
            this LoggerConfiguration loggerConfig,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            return loggerConfig
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", environment.ApplicationName)
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .WriteTo.Console();
        }
    }
}
