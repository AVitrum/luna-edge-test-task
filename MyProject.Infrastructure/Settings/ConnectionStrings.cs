namespace MyProject.Infrastructure.Settings;

/// <summary>
/// Represents the connection string settings for the application.
/// </summary>
public class ConnectionStrings
{
    /// <summary>
    /// The configuration section name for connection strings.
    /// </summary>
    public const string SectionName = "ConnectionStrings";

    /// <summary>
    /// The default database connection string.
    /// </summary>
    public string DefaultConnection { get; set; } = null!;
}