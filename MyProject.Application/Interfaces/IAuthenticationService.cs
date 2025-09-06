using MyProject.Application.Payloads;

namespace MyProject.Application.Interfaces;

/// <summary>
/// Provides methods for user registration and authentication.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Registers a new user with the specified username, email, and password.
    /// </summary>
    /// <param name="username">The username of the new user.</param>
    /// <param name="email">The email address of the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <returns>A result containing a string (e.g., token or message) if successful.</returns>
    Task<Result<string?>> RegisterAsync(string username, string email, string password);

    /// <summary>
    /// Authenticates a user using the provided identifier (username or email) and password.
    /// </summary>
    /// <param name="identifier">The username or email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A result containing a string (e.g., token) if authentication is successful.</returns>
    Task<Result<string?>> AuthenticateAsync(string identifier, string password);
}