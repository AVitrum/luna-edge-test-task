using DotNetEnv;

namespace MyProject.WebApi.Middleware;

public static class EnvironmentSetup
{
    public static string ConfigureEnvironment(WebApplicationBuilder builder)
    {
        var defaultDirectory = "/app";

        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
        {
            defaultDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..");
            Env.Load(Path.Combine(defaultDirectory, ".env.dev"));
        }

        builder.Configuration.AddEnvironmentVariables();
        return defaultDirectory;
    }
}