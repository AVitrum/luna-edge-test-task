using MyProject.Application.Interfaces;

namespace MyProject.Infrastructure.Security;

/// <summary>
/// Provides password hashing and verification using BCrypt.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Hashes the provided password using BCrypt.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>The hashed password string.</returns>
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifies a password against a hashed password using BCrypt.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hashedPassword">The hashed password to compare against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}