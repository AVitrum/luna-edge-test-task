using Serilog;
using Serilog.Events;

namespace MyProject.WebApi.Middleware;

/// <summary>
/// Provides configuration for Serilog logging in the application.
/// </summary>
public static class LoggingConfiguration
{
    /// <summary>
    /// Configures Serilog logging for the web application, including console and file outputs.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <param name="defaultDirectory">The default directory for log files.</param>
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