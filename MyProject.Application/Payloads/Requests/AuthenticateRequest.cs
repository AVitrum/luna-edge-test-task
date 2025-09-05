namespace MyProject.Application.Payloads.Requests;

public sealed class AuthenticateRequest
{
    public required string Identifier { get; init; }
    public required string Password { get; init; }
}