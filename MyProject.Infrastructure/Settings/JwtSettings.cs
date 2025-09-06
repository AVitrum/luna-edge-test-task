namespace MyProject.Infrastructure.Settings;

/// <summary>
/// Represents the JWT authentication settings for the application.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// The configuration section name for JWT settings.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// The secret key used to sign JWT tokens.
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// The issuer of the JWT token.
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// The audience for the JWT token.
    /// </summary>
    public string Audience { get; set; } = null!;
}