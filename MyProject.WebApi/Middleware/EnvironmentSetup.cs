namespace MyProject.WebApi.Middleware;

/// <summary>
/// Provides environment configuration utilities for the application.
/// </summary>
public static class EnvironmentSetup
{
    /// <summary>
    /// Determines and returns the default directory for environment setup based on whether the app is running in a container.
    /// </summary>
    /// <returns>The default directory path as a string.</returns>
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