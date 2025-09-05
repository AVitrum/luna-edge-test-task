namespace MyProject.Infrastructure.Settings;

public class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}