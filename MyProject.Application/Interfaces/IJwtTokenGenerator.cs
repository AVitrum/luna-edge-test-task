using MyProject.Domain.Entities;

namespace MyProject.Application.Interfaces;

/// <summary>
/// Provides methods for generating JWT tokens for authenticated users.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>A JWT token string.</returns>
    string GenerateToken(User user);
}