namespace MyProject.Application.Payloads.Requests;

/// <summary>
/// Request payload for user authentication.
/// </summary>
public sealed class AuthenticateRequest
{
    /// <summary>
    /// Username or email of the user.
    /// </summary>
    public required string Identifier { get; init; }
    /// <summary>
    /// User's password.
    /// </summary>
    public required string Password { get; init; }
}