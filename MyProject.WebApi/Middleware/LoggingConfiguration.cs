using Serilog;
using Serilog.Events;

namespace MyProject.WebApi.Middleware;

public static class LoggingConfiguration
{
    public static void ConfigureLogging(WebApplicationBuilder builder, string defaultDirectory)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug() 
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information) 
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .WriteTo.Console()
            .WriteTo.File($"{defaultDirectory}/logs/api.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}