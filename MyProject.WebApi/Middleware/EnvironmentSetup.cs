using DotNetEnv;

namespace MyProject.WebApi.Middleware;

public static class EnvironmentSetup
{
    public static string ConfigureEnvironment()
    {
        var defaultDirectory = "/app";

        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
        {
            defaultDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..");
        }

        return defaultDirectory;
    }
}