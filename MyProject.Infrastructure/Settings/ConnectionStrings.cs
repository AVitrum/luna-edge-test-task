namespace MyProject.Infrastructure.Settings;

public class ConnectionStrings
{
    public const string SectionName = "ConnectionStrings";
    public string DefaultConnection { get; set; } = null!;
}