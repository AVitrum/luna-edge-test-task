namespace MyProject.Application.Interfaces;

/// <summary>
/// Provides methods for hashing and verifying user passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes the specified password using a secure algorithm.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>The hashed password string.</returns>
    string Hash(string password);

    /// <summary>
    /// Verifies that the specified password matches the hashed password.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hashedPassword">The hashed password to compare against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    bool Verify(string password, string hashedPassword);
}